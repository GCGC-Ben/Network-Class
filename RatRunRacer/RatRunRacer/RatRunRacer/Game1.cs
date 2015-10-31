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
        enum GameState {mainMenu, playing, loading, paused }
        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Rat myPlayer;
        MainMenu mMenu;
        private GameState gameState;
        

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

            myPlayer = new Rat(Color.Red, new Vector2(0, 500));
            mMenu = new MainMenu(new Vector2(300, 360), new Vector2(600, 360), new Vector2(900, 360));

            gameState = GameState.mainMenu;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts\\Font1");
            Rat.load(Content);
            mMenu.load(Content);
        }

        protected override void UnloadContent()
        {
        }

 
        protected override void Update(GameTime gameTime)
        {
           
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            myPlayer.update();
            

            base.Update(gameTime);
        }

     
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend); //Start Hud draw
           
            
            //spriteBatch.DrawString(font, "Jackson likes men", new Vector2(100, 100), new Color(255, 255, 255));

            //draw main menu
            if (gameState == GameState.mainMenu)
            {
                mMenu.Draw(spriteBatch);
            }

            spriteBatch.End();//end HUD draw

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend); //Start Game Draw    TODO: add camera logic

            myPlayer.draw(spriteBatch);

            spriteBatch.End();//end game draw

            base.Draw(gameTime);
        }
    }
}
