using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RatRunRacer
{
    class OtherRats
    {
        public static Dictionary<string, Rat> Newrats = new Dictionary<string, Rat>();
        public static Dictionary<string,Rat> allrats = new Dictionary<string,Rat>();

        public static void update(World world1)
        {
            try
            {
                foreach (KeyValuePair<string, Rat> r in allrats)
                {
                    r.Value.update(world1);
                }
            }
            catch
            {

            }
        }
        public static void draw(SpriteBatch sb)
        {
            allrats = Newrats;
            try
            {
                foreach (KeyValuePair<string, Rat> r in allrats)
                {
                    r.Value.draw(sb);
                }
            }
            catch
            {

            }
        }
    }
}
