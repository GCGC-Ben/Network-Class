using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace RatRunRacer
{
    class TextManager
    {
        public static SpriteFont font;
        public static KeyboardState prevKB;

        public static void load(ContentManager content)
        {
            font = content.Load<SpriteFont>("Fonts\\Font1");
            prevKB = Keyboard.GetState();
        }
        public static string getString( string s)
        {
            string sn = s;

            Keys[] prevKeys = prevKB.GetPressedKeys(); //get keys pressed last update

            Keys[] cKeys = Keyboard.GetState().GetPressedKeys(); 

            prevKB = Keyboard.GetState(); //reset keyboard for next pass


            for (int i = 0; i < cKeys.Length; i++)
            {
                if (!prevKeys.Contains(cKeys[i]))
                {
                    string str = cKeys[i].ToString();
                    if (str.Length == 1)
                    {
                        sn += str; //add the current pressed key to the string
                    }
                    else if (str.Length == 2 && str.Contains('D'))
                    {
                       sn += str.Remove(0,1);
                    }
                    else if (str == "Back" && sn.Length >0)
                    {
                        sn =sn.Remove(sn.Length - 1);
                    }
                    else if (str == "OemPeriod")
                    {
                        sn += ".";
                    }
                }
            }

            return sn; //return the new string 99% of the time it will not change
        }

    }
}
