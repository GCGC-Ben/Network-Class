using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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

        static void Main(string[] args)
        {
            IPEndPoint end = new IPEndPoint(0,20000);
            TcpListener listen = new TcpListener(end);
           
            while (true)
            {
                listen.Start(100);
                TcpClient client = listen.AcceptTcpClient();
                allSockets.Add(client);
                clientsConnected++;
                Console.WriteLine(clientsConnected + " Clients");
                Thread t = new Thread(threadRun);
               
                t.Start();
                
                Console.WriteLine("Client Connected");
            }
            Console.Read();
        }

       

        public static void threadRun()
        {
            TcpClient accepted = allSockets.ElementAt(clientsConnected -1);
            NetworkStream strem = accepted.GetStream();
            byte[] bytes = new byte[655357];
           
            int i;
          
            string str;

            try
            {
                while ((i = strem.Read(bytes, 0, bytes.Length))!=0)
                {
                    
                    str = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Recieved: " + str);

                    str = str.ToUpper();

                    byte[] formatted = System.Text.Encoding.ASCII.GetBytes(str);

                    if (readyToGo)
                    {
                        foreach (TcpClient s in allSockets)
                        {
                            NetworkStream eachStrem = s.GetStream();
                            if (s != accepted)//don't send to self
                            {
                                eachStrem.Write(formatted, 0, formatted.Length);
                            }
                        }
                    }
                    if(readyToGo == false)
                    {
                        string[] recMessage = str.Split('$');
                        string username = "";
                        if (recMessage[1] == "Y" && !ready.Contains(recMessage[2]))
                        {
                            ready.Add(recMessage[2]);
                            username = recMessage[2];
                            users.Add(username, new ClientUser());
                            
                            numReady++;
                            
                        }
                        if (numReady == clientsConnected)
                        {
                            string start = ("4$$");
                            int allReady = 0;
                            byte[] startBytes = System.Text.Encoding.ASCII.GetBytes(start);

                            foreach (TcpClient s in allSockets)
                            {
                                NetworkStream eachStrem = s.GetStream();
                           
                                eachStrem.Write(startBytes, 0, startBytes.Length);
                                string temp = str;
                                string[] check = temp.Split('$');

                                if (check[0] == "1")
                                {
                                    if (users[check[3]].getState() != 4)
                                    {
                                        users[check[3]].setState(4);
                                    }
                                }
                            }

                            //check to see if trasmitting movement data
                           

                            foreach (KeyValuePair<string, ClientUser> u in users)
                            {
                                if (u.Value.getState() == 4)
                                {
                                    allReady++;
                                }

                            }
                            if (allReady == clientsConnected)
                            {
                                readyToGo = true;
                            }  
                       }
                    }
                    Console.WriteLine();
                }
            }
            catch
            {
            }
            accepted.Close();
            allSockets.Remove(accepted);
            clientsConnected--;
            Console.WriteLine(clientsConnected + " Clients");
        }

    }
}
