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
       
        static TcpClient client;
        static void Main(string[] args)
        {
            Thread t = new Thread(readThread);
            
            //s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client = new TcpClient(AddressFamily.InterNetwork);
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("10.90.120.81"), 20000);
            //IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 20000);
            NetworkStream strem;
            try
            {
                //s.Connect(localEndPoint);
                client.Connect(localEndPoint);
                connected = true;
                strem = client.GetStream();
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

                    strem.Write(data, 0, data.Length);
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
          
            client.Close();
        }

        public static void readThread()
        {
            try
            {
                byte[] bytes = new byte[655357];
                while (connected)
                {
                    NetworkStream stream = client.GetStream();
                    int i;
                    i = stream.Read(bytes, 0, bytes.Length);
                   

                        String str = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        byte[] buffer;
                        buffer = System.Text.Encoding.ASCII.GetBytes(str);
                        int bytesRead = client.ReceiveBufferSize;

                        string strData = Encoding.ASCII.GetString(buffer);
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
