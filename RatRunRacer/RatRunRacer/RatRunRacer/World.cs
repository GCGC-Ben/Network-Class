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
        }

        public World()
        {
            rand = new Random();

            int size = 600;
            for (int i = 0; i < size; i++) //generate bottem for whole map first
            {
                Tile t = new Tile(new Vector2(16 * i, 0), dirtTxt);
                allWorldTiles.Add(t);
                solidTiles.Add(t);
                
                for (int i2 = 0; i2 < 16; i2++)
                {
                    allWorldTiles.Add(new Tile(new Vector2(16 * i, i2 * 16 + 16), dirtBottemTxt));
                }
            }

            for(int i = 0; i < size/2; i++) //Make clouds
            {
                if (rand.Next(100) < 20)
                {
                    if (rand.Next(3) > 1)
                    {
                        Tile t = new Tile(new Vector2(32 * i, -rand.Next(500) - 100), cloudTxt);
                        allWorldTiles.Add(t);
                    }
                    else
                    {
                        Tile t = new Tile(new Vector2(32 * i, -rand.Next(500) - 100), cloud2Txt);
                        allWorldTiles.Add(t);
                    }
                }
            }

            for (int i = 0; i < size; i++) //generate the platforms
            {
                if (rand.Next(100) < 10)
                {
                    int height = -rand.Next(200) - 20;
                    for (int i2 = 0; i2 < 4; i2++)
                    {
                        Tile t = new Tile(new Vector2(16 * i, height), rockTxt);

                        solidTiles.Add(t);
                        allWorldTiles.Add(t);
                        i++;
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
