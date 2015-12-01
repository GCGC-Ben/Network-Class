using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace RatRunRacer
{
    class Lobby
    {
        static Texture2D background;
        static SpriteFont font;
        public static Rat myp ;
        static int select;
        static string serverIP ="";
        static int waitTime;
        static int state = 0;
        public static bool connected = false;
        static TcpClient client;
        static public NetworkStream strem;
        static bool canPress;
        public static Thread readerThread;
        static int amountrdy = 0;
        static string connectedUsers = "";
        static bool isReady = false;
        static bool isFinshed = false;
        static string whoWonLastGame = "No One";

        public static void load(ContentManager content)
        {
            background = content.Load<Texture2D>("Icons\\backgroundtxt");
            font = content.Load<SpriteFont>("Fonts\\Font1");
            myp=new Rat(Color.White, new Vector2(100, 500),new Vector2(0,0));
            myp.makePlayerControled();
        }

        public static bool Update()
        {
            switch (state)
            {
                case 0:
                    naming();
                    break;
                case 1:
                    return waiting();
            }

            return false;
        }

        public static void naming()
        {
            switch (select)
            {
                case 0:
                    serverIP = TextManager.getString(serverIP);
                    break;
                case 1:
                    myp.username = TextManager.getString(myp.username);
                    break;
            }

            if (waitTime == 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    if (select == 0)
                    {
                        select = 1;
                    }
                    else if (select == 1)
                    {
                        select = 0;
                    }
                    waitTime = 30;
                }
            }
            else
            {
                waitTime--;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter)&&canPress)
            {
                ConnectToServer();
                state = 1;
                canPress = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Enter))
            {
                canPress = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Down) && Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                waitTime = 0;
            }


        }
        public static bool waiting()
        {
            
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                myp.ratIsReady = true;
                myp.sendLobbyInfoToServer();
            }

            OtherRats.allrats = OtherRats.Newrats;
            foreach (KeyValuePair<string, Rat> r in OtherRats.allrats)
            {
                if (r.Value.ratIsReady)
                {
                    amountrdy++;
                }
            }
          
            if (isReady)
            {
                isReady = false;//reset isready
                return true;
            }
   
            return false;
        }

        public static bool goBackToLobby()
        {
            if(isFinshed)
            {
                //reset stuff to get ready for new lobby
                isFinshed = false; 
                myp.ratIsReady = false;
                OtherRats.allrats.Clear();
                OtherRats.Newrats.Clear();
                return true;
            }

            return false;
        }
        public static void ConnectToServer()
        {
            readerThread = new Thread(readThread);

            //s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client = new TcpClient(AddressFamily.InterNetwork);
            //IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("10.90.121.94"), 20000);
            //IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 20000);
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), 20000);
            try
            {
                //s.Connect(localEndPoint);
                client.Connect(localEndPoint);
                connected = true;
                strem = client.GetStream();
                readerThread.Start();
                byte[] data = Encoding.ASCII.GetBytes("0$"+myp.username);
                strem.Write(data, 0, data.Length);
            }
            catch
            {
                if (connected)
                {
                    Console.WriteLine("Disconnceted from server");
                    connected = false;
                }
                else
                {
                    Console.WriteLine("Failed to connect to server");
                }
            }
        }

        public static bool readLobby(){
            byte[] bytes = new byte[655357];

            try
            {
                NetworkStream stream = client.GetStream();
                int i;
                i = stream.Read(bytes, 0, bytes.Length);


                String str = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                byte[] buffer;
                buffer = System.Text.Encoding.ASCII.GetBytes(str);

                string strData = Encoding.ASCII.GetString(buffer);

                if (strData == "Start Game")
                {
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

       public static void readThread()
        {
            try
            {
                byte[] bytes = new byte[655357];
                while (connected)
                {
                    try
                    {
                        NetworkStream stream = client.GetStream();
                        int i;
                        i = stream.Read(bytes, 0, bytes.Length);


                        String str = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        byte[] buffer;
                        buffer = System.Text.Encoding.ASCII.GetBytes(str);

                        string strData = Encoding.ASCII.GetString(buffer);
                        string[] allData = strData.Split('$');
                        if (allData[0] == "0")
                        {
                            if (!OtherRats.Newrats.ContainsKey(allData[1]))
                            {
                                OtherRats.Newrats.Add(allData[1], new Rat(Color.White, new Vector2(0, 0),new Vector2(0,0)));
                            }
                            if (allData[1] == "Hello")
                            {
                                connectedUsers = allData[2];
                            }
                        }
                        if (allData[0] == "1")
                        {
                            if (OtherRats.Newrats.ContainsKey(allData[4]))
                            {
                                OtherRats.Newrats[allData.ElementAt(4)] = new Rat(Color.White,
                                  new Vector2((float)Convert.ToDouble(allData.ElementAt(1).Substring(1, allData.ElementAt(1).IndexOf('Y') - 1)),
                                      (float)Convert.ToDouble(allData.ElementAt(1).Substring(allData.ElementAt(1).IndexOf('Y') + 1,
                                      allData.ElementAt(1).Length - 1 - allData.ElementAt(1).IndexOf('Y')))), 
                                      new Vector2((float)Convert.ToDouble(allData.ElementAt(2).Substring(1, allData.ElementAt(2).IndexOf('Y') - 1)),
                                      (float)Convert.ToDouble(allData.ElementAt(2).Substring(allData.ElementAt(2).IndexOf('Y') + 1,
                                      allData.ElementAt(2).Length - 1 - allData.ElementAt(2).IndexOf('Y')))));
                                OtherRats.Newrats[allData.ElementAt(4)].username = allData[4];

                                if(allData[3] == "R")
                                {
                                     OtherRats.Newrats[allData[4]].facingRight = true;
                                }
                                else
                                {
                                     OtherRats.Newrats[allData[4]].facingRight = false;
                                }
                            }
                            else
                            {
                                OtherRats.Newrats.Add(allData[4], new Rat(Color.White,
                                  new Vector2((float)Convert.ToDouble(allData.ElementAt(1).Substring(1, allData.ElementAt(1).IndexOf('Y') - 1)),
                                      (float)Convert.ToDouble(allData.ElementAt(1).Substring(allData.ElementAt(1).IndexOf('Y') + 1,
                                      allData.ElementAt(1).Length - 1 - allData.ElementAt(1).IndexOf('Y')))), 
                                      new Vector2((float)Convert.ToDouble(allData.ElementAt(2).Substring(1, allData.ElementAt(2).IndexOf('Y') - 1)),
                                      (float)Convert.ToDouble(allData.ElementAt(2).Substring(allData.ElementAt(2).IndexOf('Y') + 1,
                                      allData.ElementAt(2).Length - 1 - allData.ElementAt(2).IndexOf('Y'))))));
                                
                                 OtherRats.Newrats[allData.ElementAt(4)].username = allData[4];
                            }
                        }
                        else if (allData[0] == "2")
                        {
                            if (!OtherRats.Newrats.ContainsKey(allData[2]))
                            {
                                OtherRats.Newrats.Add(allData[2], new Rat(Color.White, new Vector2(0, 0),new Vector2(0,0)));
                            }
                            else
                            {
                                if(allData[1] == "Y")
                                {
                                    OtherRats.Newrats[allData[2]].ratIsReady = true;
                                }
                            }
                        }
                        else if (allData[0] == "4")
                        {
                            isReady = true;
                        }
                        else if (allData[0] == "5")
                        {

                        }
                        else if (allData[0] == "6")
                        {
                            whoWonLastGame = allData[1];
                            isFinshed = true;
                        }
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {
                Console.WriteLine("Read failed");
            }
        }

        public static void Draw(SpriteBatch sb)
        {
            if (state == 0)
            {
                if (select == 0)
                {
                    sb.DrawString(font, "IP: " + serverIP, new Vector2(640, 300), Color.White, 0f,
                        new Vector2(font.MeasureString("IP: " + serverIP).X / 2, 0), 1f, SpriteEffects.None, 0f);
                    sb.DrawString(font, "UserName: " + myp.username, new Vector2(640, 400), new Color(200, 200, 200), 0f,
                        new Vector2(font.MeasureString("UserName: " + myp.username).X / 2, 0), 1f, SpriteEffects.None, 0f);
                }
                else
                {
                    sb.DrawString(font, "IP: " + serverIP, new Vector2(640, 300), new Color(200, 200, 200), 0f,
                        new Vector2(font.MeasureString("IP: " + serverIP).X / 2, 0), 1f, SpriteEffects.None, 0f);
                    sb.DrawString(font, "UserName: " + myp.username, new Vector2(640, 400), Color.White, 0f,
                        new Vector2(font.MeasureString("UserName: " + myp.username).X / 2, 0), 1f, SpriteEffects.None, 0f);
                }
                sb.DrawString(font, "Press ENTER To Connect to server", new Vector2(640, 600),
                    Color.White, 0f, new Vector2(font.MeasureString("Press ENTER To Connect to server").X / 2, 0), 1f, SpriteEffects.None, 0f);
            }
            else
            {
                sb.DrawString(font, "Press R to Ready Up", new Vector2(640, 300),
                    Color.White, 0f, new Vector2(font.MeasureString("Press R To Ready Up").X / 2, 0), 1f, SpriteEffects.None, 0f);

                sb.DrawString(font, amountrdy+"/"+(OtherRats.allrats.Count+1)+" Players Ready", new Vector2(640, 350),
                    Color.White, 0f, new Vector2(font.MeasureString(amountrdy + "/" + connectedUsers + " Players Ready").X / 2, 0), 1f, SpriteEffects.None, 0f);

                sb.DrawString(font, "Connected To Server Waiting on players", new Vector2(640, 600),
                   Color.White, 0f, new Vector2(font.MeasureString("Connected To Server Waiting on players").X / 2, 0), 1f, SpriteEffects.None, 0f);

               sb.DrawString(font, whoWonLastGame +"-Won the last game", new Vector2(640, 400),
                   Color.White, 0f, new Vector2(font.MeasureString( whoWonLastGame +" Won the last game").X / 2, 0), 1f, SpriteEffects.None, 0f);
            }


            sb.Draw(background, new Vector2(420, 210), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);
        }

    }
}
