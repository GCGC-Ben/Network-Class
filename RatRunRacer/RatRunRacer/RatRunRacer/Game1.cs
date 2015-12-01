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

            mainMenu = new MainMenu(new Vector2(640, 280), new Vector2(640, 380));
            myPlayer = new Rat(Color.White, new Vector2(100, 500),new Vector2(0,0));

            cam = new Camera2d();
            cam._pos.Y = -200;
            cam._pos.X = 650;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts\\Font1");
            TextManager.load(Content);
            Lobby.load(Content);
            Rat.load(Content);
            MainMenu.load(Content);
            mainMenu.generateMenu();
            World.load(Content);
            World1 = new World();
        }

        protected override void UnloadContent()
        {
            if (Lobby.readerThread != null)
            {
                Lobby.readerThread.Abort();//kill the thread 
            }
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            if (state == GameState.lobby)
            {
                if (Lobby.Update())
                {
                    myPlayer = Lobby.myp;
                    state = GameState.playing;
                }
            }

            if (state == GameState.playing)
            {
                myPlayer.update(World1);
                OtherRats.update(World1);

                if (myPlayer.pos.X > 650)
                {
                    cam._pos.X = myPlayer.pos.X;
                }
                else
                {
                    cam._pos.X = 650;
                }
                if (myPlayer.pos.Y < 1235)
                {
                    cam._pos.Y = myPlayer.pos.Y;
                }
                else
                {
                    cam._pos.Y = 1235;
                }
                World1.update();

                if (Lobby.goBackToLobby())
                {
                    state = GameState.lobby;
                }

            }
            //Selection switch case for main menu
            if (state == GameState.MMenu)
            {
                mainMenu.Update();

                if ( ks.IsKeyDown(Keys.Enter))
                {
                    switch (mainMenu.getSelection())
                    {
                        case 1:
                            state = GameState.lobby;
                            break;
                        case 2:
                            this.Exit();
                            break;

                        default:
                            break;
                    }
                }
            }

            base.Update(gameTime);
        }

     
        protected override void Draw(GameTime gameTime)
        {
            if (state == GameState.playing)
            {
                GraphicsDevice.Clear(Color.SkyBlue);
            }
            else
            {
                GraphicsDevice.Clear(new Color(30,30,30));
            }

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend); //Start Hud draw

            if (state == GameState.MMenu)
            {
                mainMenu.Draw(spriteBatch);
            }

            if (state == GameState.lobby)
            {
                Lobby.Draw(spriteBatch);
            }
            spriteBatch.End();//end HUD draw


            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,null,null,null,null,cam.get_transformation(device)); //Start Game Draw    TODO: add camera logic

            if (state == GameState.playing)
            {
                myPlayer.draw(spriteBatch);
                World1.draw(spriteBatch, cam.Pos);
                OtherRats.draw(spriteBatch);
            }
            spriteBatch.End();//end game draw


            base.Draw(gameTime);
        }
    }
}
