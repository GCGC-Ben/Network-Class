using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        static Rat myp;
        static int select;
        static string serverIP ="";
        static int waitTime;
        static int state = 0;

        public static void load(ContentManager content)
        {
            background = content.Load<Texture2D>("Icons\\backgroundtxt");
            font = content.Load<SpriteFont>("Fonts\\Font1");
            myp = new Rat(Color.White,new Vector2(0,0));
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

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                ConnectToServer();
                state = 1;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Down) && Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                waitTime = 0;
            }


        }
        public static bool waiting()
        {
            return true;
        }

        public static void ConnectToServer()
        {

        }

        public static void Draw(SpriteBatch sb)
        {
            if (select == 0)
            {
                sb.DrawString(font, "IP: " + serverIP, new Vector2(640, 300), Color.White, 0f,
                    new Vector2(font.MeasureString("IP: " + serverIP).X / 2, 0), 1f, SpriteEffects.None, 0f);
                sb.DrawString(font, "UserName: "+ myp.username, new Vector2(640, 400), new Color(200,200,200), 0f, 
                    new Vector2(font.MeasureString("UserName: "+myp.username).X / 2, 0), 1f, SpriteEffects.None, 0f);
            }
            else 
            {
                sb.DrawString(font, "IP: " + serverIP, new Vector2(640, 300), new Color(200, 200, 200), 0f, 
                    new Vector2(font.MeasureString("IP: " + serverIP).X / 2, 0), 1f, SpriteEffects.None, 0f);
                sb.DrawString(font,"UserName: " + myp.username, new Vector2(640, 400), Color.White, 0f, 
                    new Vector2(font.MeasureString("UserName: "+myp.username).X / 2, 0), 1f, SpriteEffects.None, 0f);
            }
            sb.DrawString(font, "Press ENTER To Connect to server", new Vector2(640, 600),
                Color.White, 0f, new Vector2(font.MeasureString("Press ENTER To Connect to server").X / 2, 0), 1f, SpriteEffects.None, 0f);

            sb.Draw(background, new Vector2(420, 210), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);
        }

    }
}
