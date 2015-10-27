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
        Vector2 pos;
        static Texture2D txt;
        Color c;
        Vector2 acc;
        Vector2 vel;
        bool onGround;

        public Rat(Color c, Vector2 startPos)
        {
            this.c = c;
            pos = startPos;
        }

        public static void load(ContentManager content)
        {
            txt = content.Load<Texture2D>("Player\\rat");
        }

        public void update()
        {
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.Right))
            {
                acc.X += .2f;
            }
            if (kb.IsKeyDown(Keys.Left))
            {
                acc.X -= .2f;
            }

            if (kb.IsKeyDown(Keys.Space) && onGround)
            {
                acc.Y = -5f;
            }

            if (pos.Y < 500)
            {
                acc.Y += .3f;
                onGround = false;
            }
            else
            {
                pos.Y = 500;
                onGround = true;
                vel.Y = 0;
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
            if (vel.X < -.1f)
            {
                vel.X += .1f;
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
            sb.Draw(txt, pos, null, c, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, .5f);
        }

    }
}
