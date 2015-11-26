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
    class Rat
    {
        public Vector2 pos;
        static Texture2D txt;
        static SpriteFont font;
        Color c;
        Vector2 acc;
        Vector2 vel;
        bool onGround;
        Rectangle bb;
        bool facingRight;
        bool justHitGround;
        Vector2 startPos;
        public bool ratIsReady;
        public string username {get;set;}
        bool isControled;

        public Rat(Color c, Vector2 startPos,Vector2 cVel)
        {
            this.c = c;
            pos = startPos;
            this.startPos = startPos;
            username = "";
            facingRight = true;
            vel = cVel;
        }

        public static void load(ContentManager content)
        {
            txt = content.Load<Texture2D>("Player\\rat");
            font = content.Load<SpriteFont>("Fonts\\Font1");
        }

        public void makePlayerControled()
        {
            isControled = true;
        }

        public void update(World world1)
        {

            KeyboardState kb = Keyboard.GetState();

            bb = new Rectangle((int)pos.X-32,(int)pos.Y-16,64,17);
            onGround = false;
            justHitGround = false;
            foreach (Tile t in world1.solidTiles) //super intensive consider a map probally fine though
            {
                if (pos.Y <= t.getBB().Bottom && t.getBB().Intersects(bb)&& vel.Y > 0)
                {
                    pos.Y = t.pos.Y;            
                    onGround = true;
                    vel.Y = 0;
                }

                if (t.getBB().Intersects(bb))
                {
                    vel.Y = 0;
                }
            }

            if (isControled)
            {
                if (kb.IsKeyDown(Keys.Right))
                {
                    acc.X += .2f;
                    facingRight = true;
                }
                if (kb.IsKeyDown(Keys.Left))
                {
                    acc.X -= .2f;
                    facingRight = false;
                }

                if ((kb.IsKeyDown(Keys.Space) || kb.IsKeyDown(Keys.Up)) && onGround)
                {
                    acc.Y = -13f;
                    onGround = false;
                }
            }

            if (!onGround)
            {
                acc.Y += .3f;
                onGround = false;
            }

            ResolveForces(world1);

            if (pos.Y > 1600) //you fell off the map reset at start 
            {
                pos = startPos;
            }
            if (Lobby.connected &&isControled)
            {
                sendPosToServer();
            }
        }
        void sendPosToServer() //think about giving this its own thread as to not lag your game
        {
            byte[] data = Encoding.ASCII.GetBytes("1$" + "x"+pos.X+"y"+pos.Y+"$"+"x"+vel.X+"y"+vel.Y+"$"+username+"$");
            Lobby.strem.Write(data, 0, data.Length);
        }

        public void sendLobbyInfoToServer()
        {
            byte[] data;
            if (ratIsReady)
            {
                data = Encoding.ASCII.GetBytes("2$" +"Y"+"$" + username + "$");
            }
            else
            {
                data = Encoding.ASCII.GetBytes("2$" +"N"+ "$" + username + "$");
            }
            Lobby.strem.Write(data, 0, data.Length);
        }

        void ResolveForces(World world1)
        {
            vel+= acc;
            acc = new Vector2(0, 0);
            

            //simulate friction
            if (vel.X > .1f)
            {
                vel.X -= .1f;
            }
            else if (vel.X < -.1f)
            {
                vel.X += .1f;
            }
            else
            {
                vel.X = 0;
            }
            if (vel.Y > .1f)
            {
                vel.Y -= .1f;
            }
            if (vel.Y < -.1f)
            {
                vel.Y += .1f;
            }

            moveHere(pos + vel,world1);
        }

        void moveHere(Vector2 npos,World world1)
        {
            bool iCanMoveHere = true;

            Rectangle nbb = new Rectangle((int)npos.X - 32, (int)npos.Y - 16, 64, 12);

            if (!justHitGround) //skip this for when you hit the ground
            {
                foreach (Tile t in world1.solidTiles) //same as intensive above but it seems to be fine
                {
                    if (nbb.Intersects(t.getBB()))
                    {
                        iCanMoveHere = false;
                        vel = new Vector2(0, 0);
                    }
                }
            }
            if (iCanMoveHere)
            {
                pos = npos;
            }
        }

        public void draw(SpriteBatch sb)
        {
            if (facingRight)
            {
                sb.Draw(txt, pos, null, c, 0f, new Vector2(txt.Width / 2, txt.Height), 1f, SpriteEffects.None, .5f);
            }
            else
            {
                sb.Draw(txt, pos, null, c, 0f, new Vector2(txt.Width / 2, txt.Height), 1f, SpriteEffects.FlipHorizontally, .5f);
            }
            sb.DrawString(font, username, pos + new Vector2(0, -90), Color.White, 0, new Vector2(font.MeasureString(username).X / 2, 0), 1f, SpriteEffects.None, 0);

            //sb.Draw(txt, bb, Color.Red); // debuging bounding box
        }

    }
}
