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
        enum GameState {MMenu,playing,lobby,pause }
        GameState state;
        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Rat myPlayer;
        Camera2d cam;
        World World1;
        MainMenu mainMenu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            device = graphics.GraphicsDevice;
            state = GameState.MMenu;
            this.Window.Title = "Rat Run Racer";
            graphics.PreferredBackBufferWidth = 1280;//720p
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            myPlayer = new Rat(Color.White, new Vector2(0, -10));
            mainMenu = new MainMenu(new Vector2(640, 260), new Vector2(640, 360), new Vector2(640, 460));

            cam = new Camera2d();
            cam._pos.Y = -200;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts\\Font1");
            Rat.load(Content);
            MainMenu.load(Content);
            mainMenu.generateMenu();
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
            KeyboardState ks = Keyboard.GetState();
            myPlayer.update(World1);
            mainMenu.Update();
            cam._pos.X = myPlayer.pos.X;
            World1.update();

            //Selection switch case for main menu
            if (state == GameState.MMenu && ks.IsKeyDown(Keys.Enter))
            {
                switch (mainMenu.getSelection())
                {
                    case 1:
                        state = GameState.lobby;
                        break;
                    case 2:
                        state = GameState.playing;
                        break;
                    case 3:
                        state = GameState.pause;
                        break;
                    default:
                        break;
                }
            }

            base.Update(gameTime);
        }

     
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);


            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend); //Start Hud draw

            if (state == GameState.MMenu)
            {
                //spriteBatch.DrawString(font, "Jackson likes men a lot and he denies it", new Vector2(100, 100), new Color(255, 255, 255));
                mainMenu.Draw(spriteBatch);
            }
            spriteBatch.End();//end HUD draw


            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,null,null,null,null,cam.get_transformation(device)); //Start Game Draw    TODO: add camera logic

            if (state == GameState.playing)
            {
                myPlayer.draw(spriteBatch);
                World1.draw(spriteBatch, cam.Pos);
            }
            spriteBatch.End();//end game draw


            base.Draw(gameTime);
        }
    }
}
