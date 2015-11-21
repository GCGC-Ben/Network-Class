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

namespace RatRunRacer{
//Class creates a menu for the game to start on
    class MainMenu
    {
        Menu main = new Menu();

        //result after pressing a button
        String buttonResult;

        //tetures for buttons
        static Texture2D startTxt;
        static Texture2D exitTxt;
        static Texture2D selectArrow;
        static Texture2D background;

        //Button positions
        Vector2 startPos;
        Vector2 optionsPos;
        Vector2 arrowPos;

        //Button areas

        //select value
        int selection;
        int counter;
        bool startWait;

        public MainMenu(Vector2 sPos, Vector2 oPos){
            startPos = sPos;
            optionsPos = oPos;
            buttonResult = "";
            arrowPos = sPos;
            selection = 1;
            counter = 30;
            startWait = false;
        }


        public static void load(ContentManager content)
        {
            startTxt = content.Load<Texture2D>("Icons\\joingame");
            exitTxt = content.Load<Texture2D>("Icons\\exitgame");
            selectArrow = content.Load<Texture2D>("Icons\\select");
            background = content.Load<Texture2D>("Icons\\backgroundtxt");
        }


        //Add all the buttons to main's arraylist of buttons
        public void generateMenu()
        {
            main.addButton(startTxt, startPos, Color.White);
            main.addButton(exitTxt, optionsPos, Color.White);
        }

        //get the selection
        public int getSelection()
        {
            return selection;
        }

        //not sure what needs to bu updated with menu unless we want fancy stuff
        //also contains logic for commands
        public void Update()
        {
            KeyboardState keyState = Keyboard.GetState();


            if (keyState.IsKeyDown(Keys.Down) && startWait != true)
                {

                    selection++;
                    if (selection == 3)
                    {
                        selection = 1;
                    }

                    switch (selection)
                    {
                        case 1:
                            arrowPos.Y = startPos.Y;
                            break;
                        case 2:
                            arrowPos.Y = optionsPos.Y;
                            break;
                    }
                    startWait = true;
                }

                if (keyState.IsKeyDown(Keys.Up)  && startWait != true)
                {
                    selection--;
                    if (selection ==0)
                    {
                        selection = 2;
                    }
                    switch (selection)
                    {
                        case 1:
                            arrowPos.Y = startPos.Y;
                            break;
                        case 2:
                            arrowPos.Y = optionsPos.Y;
                            break;
                    }
                    startWait = true;
                }

                if(startWait == true)
                {
                    counter--;
                }

                if (keyState.IsKeyUp(Keys.Up) && keyState.IsKeyUp(Keys.Down))
                {
                    counter = 30;
                    startWait = false;
                }

                if (counter == 0)
                {
                    counter = 30;
                    startWait = false;
                }
                
          
        }


        public void Draw(SpriteBatch sb)
        {
            main.Draw(sb);
            sb.Draw(selectArrow, arrowPos, null, Color.White, 0f, new Vector2(selectArrow.Width/2,0), 1f, SpriteEffects.None, .0f);
            sb.Draw(background, new Vector2(420,210), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);
        }

    }
}