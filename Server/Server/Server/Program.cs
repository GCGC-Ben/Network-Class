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
        static List<Socket> allSockets = new List<Socket>();

        static void Main(string[] args)
        {

            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Bind(new IPEndPoint(0, 20000));

            while (true)
            {
                s.Listen(100);
                Socket accepted = s.Accept();
                allSockets.Add(accepted);
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
            Socket accepted = allSockets.ElementAt(clientsConnected - 1);
            bool exit = false;
            try
            {
                while (!exit)
                {
                    buffer = new byte[accepted.SendBufferSize];
                    int bytesRead = accepted.Receive(buffer);

                    byte[] formatted = new byte[bytesRead];

                    for (int i = 0; i < bytesRead; i++)
                    {
                        formatted[i] = buffer[i];
                    }
                    string strData = Encoding.ASCII.GetString(formatted);

                    foreach (Socket s in allSockets)
                    {
                        if (s != accepted)//don't send to self
                        {
                            s.Send(formatted);
                        }
                    }

                    Console.WriteLine(strData);
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
