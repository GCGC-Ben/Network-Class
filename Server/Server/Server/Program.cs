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
        static byte[] buffer;
        static int clientsConnected;
        static Socket s;
        static List<TcpClient> allSockets = new List<TcpClient>();

        static void Main(string[] args)
        {

            //s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //s.Bind(new IPEndPoint(0, 20000));
            IPEndPoint end = new IPEndPoint(0,20000);
            TcpListener listen = new TcpListener(end);

            while (true)
            {
                listen.Start(100);
                //Socket accepted = s.Accept();
                //allSockets.Add(accepted);
                TcpClient client = listen.AcceptTcpClient();
                allSockets.Add(client);
                clientsConnected++;
                Console.WriteLine(clientsConnected + " Clients");
                Thread t = new Thread(newUser);
                t.Start();
                Console.WriteLine("Client Connected");
            }
            Console.Read();
            s.Close();


        }

        public static void newUser()
        {
            //Socket accepted = allSockets.ElementAt(clientsConnected - 1);
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
                    //buffer = new byte[accepted.ReceiveBufferSize];
                    //int bytesRead = strem.;

                    str = str.ToUpper();

                    byte[] formatted = System.Text.Encoding.ASCII.GetBytes(str);
                    //strem.Read(buffer,0, accepted.ReceiveBufferSize);

                    //string strData = Encoding.ASCII.GetString(buffer);

                    foreach (TcpClient s in allSockets)
                    {
                        NetworkStream eachStrem = s.GetStream();
                        if (s != accepted)//don't send to self
                        {
                            eachStrem.Write(formatted,0,formatted.Length);
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
