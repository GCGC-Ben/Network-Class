using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{

    class Program
    {
        static Socket s;
        static bool connected;
       
        static void Main(string[] args)
        {
            Thread t = new Thread(readThread);
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 20000);

            try
            {
                s.Connect(localEndPoint);
                connected = true;
                t.Start();

                Console.Write("Enter your name: ");
                string name = Console.ReadLine();
                while (connected)
                {
                    string text = name + ":";
                    string writtenText = Console.ReadLine();
                    text += writtenText + "\n";

                    if (writtenText == "exit")
                    {
                        break;
                    }

                    byte[] data = Encoding.ASCII.GetBytes(text);

                    s.Send(data);
                }
            }
            catch
            {
                if (connected)
                {
                    Console.WriteLine("Disconnceted from server");
                }
                else
                {
                    Console.WriteLine("Failed to connect to server");
                }
            }
            connected = false;
            Console.Read();
            s.Close();

        }

        public static void readThread()
        {
            try
            {
                while (connected)
                {
                    byte[] buffer;
                    buffer = new byte[s.SendBufferSize];
                    int bytesRead = s.Receive(buffer);

                    byte[] formatted = new byte[bytesRead];

                    for (int i = 0; i < bytesRead; i++)
                    {
                        formatted[i] = buffer[i];
                    }
                    string strData = Encoding.ASCII.GetString(formatted);
                    Console.WriteLine(strData);
                }
            }
            catch
            {
                Console.WriteLine("Read failed");
            }
        }
    }
}
