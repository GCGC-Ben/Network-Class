using System;
using System.Collections.Generic;
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

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Rat myPlayer;
        Camera2d cam;
        World World1;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            device = graphics.GraphicsDevice;

            this.Window.Title = "Rat Run Racer";
            graphics.PreferredBackBufferWidth = 1280;//720p
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            myPlayer = new Rat(Color.White, new Vector2(0, -10));

            cam = new Camera2d();
            cam._pos.Y = -200;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts\\Font1");
            Rat.load(Content);

            World.load(Content);
            World1 = new World();
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
           
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            myPlayer.update(World1);
            cam._pos.X = myPlayer.pos.X;
            World1.update();


            base.Update(gameTime);
        }

     
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);


            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend); //Start Hud draw

            //spriteBatch.DrawString(font, "Jackson likes men a lot and he denies it", new Vector2(100, 100), new Color(255, 255, 255));

            spriteBatch.End();//end HUD draw


            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,null,null,null,null,cam.get_transformation(device)); //Start Game Draw    TODO: add camera logic

            myPlayer.draw(spriteBatch);
            World1.draw(spriteBatch, cam.Pos);

            spriteBatch.End();//end game draw


            base.Draw(gameTime);
        }
    }
}
