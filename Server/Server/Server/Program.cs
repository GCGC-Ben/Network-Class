using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;

namespace Server
{
    class Program
    {
        static int clientsConnected;
        static List<TcpClient> allSockets = new List<TcpClient>();
        static Dictionary<string, ClientUser> users = new Dictionary<string, ClientUser>();
        static int numReady = 0;
        static bool readyToGo = false;
        static List<string> ready = new List<string>();
        static ArrayList winners = new ArrayList();
        static int stageNum = 1;
        static bool stageSel = true;

        static void Main(string[] args)
        {
            //set port info
            IPEndPoint end = new IPEndPoint(0, 20000);
            //set up lister server
            TcpListener listen = new TcpListener(end);

            while (true)
            {
                listen.Start(100);
                //accept clients
                TcpClient client = listen.AcceptTcpClient();
                //add to client list
                allSockets.Add(client);
                clientsConnected++;
                Console.WriteLine(clientsConnected + " Clients");
                //create thread
                Thread t = new Thread(threadRun);
                //begin the thread
                t.Start();

                Console.WriteLine("Client Connected");
            }
        }


        //main thread for a client 
        public static void threadRun()
        {
            //get current client
            TcpClient accepted = allSockets.ElementAt(clientsConnected - 1);
            //get client's stream
            NetworkStream strem = accepted.GetStream();
            byte[] bytes = new byte[655357];

            //holds data for transfer
            int i;
            //string for data
            string str;

            try
            {
                while ((i = strem.Read(bytes, 0, bytes.Length)) != 0)
                {
                    //convert the recieved bytes into string for printing and transmitting to other clients
                    str = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Recieved: " + str);
                    //formatting
                    str = str.ToUpper();

                    //main response array
                    byte[] formatted = System.Text.Encoding.ASCII.GetBytes(str);

                    //split the string to handle different requests 
                    //0 -connection, 1-playing, 2-lobby, 3-winning, 4-readyToStart, 5-stage info
                    string[] recieved = str.Split('$');

                    //if in greeting stage
                    if (recieved[0] == "0")
                    {
                        //send out stage info
                        if (stageSel)
                        {
                            //format info into bytes
                            byte[] stage = Encoding.ASCII.GetBytes("5$" + stageNum + "$");
                            //send to clients
                            foreach (TcpClient s in allSockets)
                            {
                                NetworkStream eachStrem = s.GetStream();
                                if (s != accepted)//don't send to self
                                {
                                    eachStrem.Write(stage, 0, stage.Length);
                                }
                            }
                            //end the stage selection
                            stageSel = false;
                        }
                        //format the hello message to get the number of clients, 
                        //weird bug in the client where the number of players that are connected never change from 1 
                        //even when set to 2   
                        byte[] hello = Encoding.ASCII.GetBytes("0$Hello$" + clientsConnected + "$" + recieved[1] + "$");
                        //send to clients
                        foreach (TcpClient s in allSockets)
                        {
                            NetworkStream eachStrem = s.GetStream();
                            if (s != accepted)//don't send to self
                            {
                                eachStrem.Write(hello, 0, hello.Length);
                            }
                        }
                    }
                    //if recieving finish line crossed
                    if (recieved[0] == "3")
                    {
                        finshLineCalcs(recieved);
                    }

                    //send messages to all clients
                    foreach (TcpClient s in allSockets)
                    {
                        if (s != accepted)
                        {
                            NetworkStream eachStrem = s.GetStream();
                            eachStrem.Write(formatted, 0, formatted.Length);
                        }

                    }

                    if (readyToGo)
                    {
                       readyToGo = false; // you sent it so reset the stat
                       numReady = 0;
                    }

                    //If still waiting on players
                    if (readyToGo == false)
                    {
                        string[] recMessage = str.Split('$');
                        string username = "";
                        if (recMessage[1] == "Y" && !ready.Contains(recMessage[2]) &&
                            recMessage[0] == "2" && !users.ContainsKey(username))
                        {
                            ready.Add(recMessage[2]);
                            username = recMessage[2];
                            users.Add(username, new ClientUser());

                            numReady++;

                        }
                        //if all players are ready begin the game
                        if (numReady >= clientsConnected)
                        {
                            string start = ("4$$");
                            int allReady = 0;
                            byte[] startBytes = System.Text.Encoding.ASCII.GetBytes(start);

                            //send to client
                            foreach (TcpClient s in allSockets)
                            {
                                NetworkStream eachStrem = s.GetStream();

                                eachStrem.Write(startBytes, 0, startBytes.Length);
                                string temp = str;
                                string[] check = temp.Split('$');

                                if (check[0] == "1")
                                {
                                    if (users[check[4]].getState() != 4)
                                    {
                                        users[check[4]].setState(4);
                                    }
                                }
                            }

                            //check to see if trasmitting movement data
                            //make sure everyone starts at the same time
                            foreach (KeyValuePair<string, ClientUser> u in users)
                            {
                                if (u.Value.getState() == 4)
                                {
                                    allReady++;
                                }

                            }
                            //begin the race mode transfer
                            if (allReady == clientsConnected)
                            {
                                readyToGo = true;
                            }
                        }
                    }


                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            accepted.Close();
            allSockets.Remove(accepted);
            clientsConnected--;
            Console.WriteLine(clientsConnected + " Clients");
        }

        static void finshLineCalcs(string[] recieved)
        {

            string win = "6$";
            //add to win array
            if (!winners.Contains(recieved[2]))
            {
                winners.Add(recieved[2]);
            }

            if (winners.Count == users.Count) //only send out the finsh message when everyone finshes 
            {
                //format the data to send 6$first$second$third$....$^$
                foreach (string w in winners)
                {
                    win += (w + "$");
                }
                win += "$^$";
                //convert to bytes to send
                byte[] winList = Encoding.ASCII.GetBytes(win);
                //send the winner list to clients

                foreach (TcpClient s in allSockets)
                {
                    NetworkStream eachStrem = s.GetStream();
                    eachStrem.Write(winList, 0, winList.Length);
                }
                //set data for next stage
                if (stageNum < 2)
                {
                    stageNum++;
                    stageSel = true;
                }
                //restart the stage selection
                else
                {
                    stageNum = 1;
                    stageSel = true;
                }

                //reset users
                winners.Clear();
                ready.Clear();
                numReady = 0;
                foreach (KeyValuePair<string, ClientUser> u in users)
                {
                    u.Value.setState(0);
                }

            }
        }

    }
}