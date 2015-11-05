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
        Color c;
        Vector2 acc;
        Vector2 vel;
        bool onGround;
        Rectangle bb;
        bool facingRight;

        public Rat(Color c, Vector2 startPos)
        {
            this.c = c;
            pos = startPos;
        }

        public static void load(ContentManager content)
        {
            txt = content.Load<Texture2D>("Player\\rat");
        }

        public void update(World world1)
        {
            KeyboardState kb = Keyboard.GetState();

            bb = new Rectangle((int)pos.X-32,(int)pos.Y-16,64,17);
            onGround = false;
            foreach (Tile t in world1.solidTiles) //super intensive consider a map
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

            if ((kb.IsKeyDown(Keys.Space)||kb.IsKeyDown(Keys.Up)) && onGround)
            {
                acc.Y = -13f;
                onGround = false;
            }

            if (!onGround)
            {
                acc.Y += .3f;
                onGround = false;
            }
          


            ResolveForces();
        }

        void ResolveForces()
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

            pos += vel;
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
            //sb.Draw(txt, bb, Color.Red); bounding box
        }

    }
}
