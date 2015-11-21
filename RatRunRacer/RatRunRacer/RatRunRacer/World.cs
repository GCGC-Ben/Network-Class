using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace RatRunRacer
{
    class World
    {
        public static Texture2D dirtTxt,dirtBottemTxt,cloudTxt,cloud2Txt,rockTxt;
        public static Texture2D level1Map;
        Random rand;

        List<Tile> allWorldTiles = new List<Tile>();
        public List<Tile> solidTiles = new List<Tile>();

        public static void load(ContentManager content)
        {
            dirtTxt = content.Load<Texture2D>("Tiles\\Dirt");
            dirtBottemTxt = content.Load<Texture2D>("Tiles\\DirtBottem");
            cloudTxt = content.Load<Texture2D>("Tiles\\Cloud");
            cloud2Txt = content.Load<Texture2D>("Tiles\\Cloud2");
            rockTxt = content.Load<Texture2D>("Tiles\\Rock");
            level1Map = content.Load<Texture2D>("Level\\Level1");
        }

        public World()
        {
            rand = new Random();

            Color[] levelColors = new Color[200000];
            level1Map.GetData(levelColors);

            for (int y = 0; y < level1Map.Height; y++)
            {
                for(int x = 0; x<level1Map.Width; x++)
                {
                switch (levelColors[x+(y*level1Map.Width)].R)
                {
                    case 255:
                        if (levelColors[x + (y * level1Map.Width)].G != 255)
                        {
                         allWorldTiles.Add(new Tile(new Vector2(x*16,y*16),dirtTxt));
                         solidTiles.Add(new Tile(new Vector2(x * 16, y * 16), dirtTxt));
                        }
                        else
                        {
                         allWorldTiles.Add(new Tile(new Vector2(x*16,y*16),dirtBottemTxt));
                        }
                        break;
                    case 0:
                        if (levelColors[x + (y * level1Map.Width)].A == 255)
                        {
                            allWorldTiles.Add(new Tile(new Vector2(x*16,y*16),dirtTxt));
                            solidTiles.Add(new Tile(new Vector2(x * 16, y * 16), dirtTxt));
                        }
                        break;
                }
                }
            }

        }

        public void update()
        {

        }

        public void draw(SpriteBatch sb,Vector2 camPos)
        {
            foreach (Tile t in allWorldTiles)
            {
                if ((t.pos.X > (camPos.X - 660)) &&(t.pos.X < (camPos.X + 660))) //really process intensive maybe put into a map if it is too slow
                {
                    t.draw(sb);
                }
            }
        }


    }

    class Tile
    {
        Texture2D txt;
        public Vector2 pos; //top middle of texture

        public Tile(Vector2 pos, Texture2D txt)
        {
            this.pos = pos;
            this.txt = txt;
        }

        public void draw(SpriteBatch sb)
        {
            sb.Draw(txt, pos, null, Color.White, 0f, new Vector2(8, 0), 1f, SpriteEffects.None, .9f);

            //sb.Draw(txt, getBB(), Color.Blue); bounding boxes
        }

        public Rectangle getBB()
        {
            return new Rectangle((int)pos.X-8,(int)pos.Y,16,16); 
        }
    }
}
