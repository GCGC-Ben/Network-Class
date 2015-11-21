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
        static Texture2D optionsTxt;
        static Texture2D connectTxt;
        static Texture2D selectArrow;
        
        //Button positions
        Vector2 startPos;
        Vector2 optionsPos;
        Vector2 connectPos;
        Vector2 arrowPos;

        //Button areas
        Rectangle startRec;
        Rectangle optionRec;
        Rectangle connectRec;

        //select value
        int selection;
        int counter;
        bool startWait;

        public MainMenu(Vector2 sPos, Vector2 oPos, Vector2 cPos){
            startPos = sPos;
            optionsPos = oPos;
            connectPos = cPos;
            buttonResult = "";
            arrowPos = new Vector2(sPos.X,sPos.Y);
            selection = 1;
            counter = 30;
            startWait = false;
        }


        public static void load(ContentManager content)
        {
            startTxt = content.Load<Texture2D>("Player\\rat");
            optionsTxt = content.Load<Texture2D>("Player\\rat");
            connectTxt = content.Load<Texture2D>("Player\\rat");
            selectArrow = content.Load<Texture2D>("Icons\\arrow");
        }


        //Add all the buttons to main's arraylist of buttons
        public void generateMenu()
        {
            main.addButton(startTxt, startPos, Color.Blue);
            main.addButton(optionsTxt, optionsPos, Color.Green);
            main.addButton(connectTxt, connectPos, Color.Purple);
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


            if (keyState.IsKeyDown(Keys.Down) && selection <= 3 && startWait != true)
                {

                    selection++;

                    switch (selection)
                    {
                        case 1:
                            arrowPos.Y = startPos.Y;
                            break;
                        case 2:
                            arrowPos.Y = optionsPos.Y;
                            break;
                        case 3:
                            arrowPos.Y = connectPos.Y;
                            break;
                        default:
                            break;
                    }
                    startWait = true;
                }

                if (keyState.IsKeyDown(Keys.Up) && selection >= 1 && startWait != true)
                {
                    selection--;
                    switch (selection)
                    {
                        case 1:
                            arrowPos.Y = startPos.Y;
                            break;
                        case 2:
                            arrowPos.Y = optionsPos.Y;
                            break;
                        case 3:
                            arrowPos.Y = connectPos.Y;
                            break;
                        default:
                            break;
                    }
                    startWait = true;
                }
                if(startWait == true)
                {
                    counter--;
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
            sb.Draw(selectArrow, arrowPos, null, Color.White, 0f, new Vector2(selectArrow.Width / 2, selectArrow.Height), 1f, SpriteEffects.None, .5f);
        }

    }
}