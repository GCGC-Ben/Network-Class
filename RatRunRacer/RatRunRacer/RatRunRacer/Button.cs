using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RatRunRacer
{
    //button class for menus and hud
    class Button
    {
        Texture2D butTxt;
        Vector2 butPos;
        Color c;

        public Button(Texture2D txt, Vector2 pos, Color col)
        {
            butTxt = txt;
            butPos = pos;
            c = col;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(butTxt, butPos, null, c, 0f, new Vector2(butTxt.Width/2, 0), 1f, SpriteEffects.None, .4f);
        }
    }
}