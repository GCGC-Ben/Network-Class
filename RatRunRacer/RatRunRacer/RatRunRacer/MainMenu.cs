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
        Texture2D startTxt;
        Texture2D optionsTxt;
        Texture2D connectTxt;
        
        //Button positions
        Vector2 startPos;
        Vector2 optionsPos;
        Vector2 connectPos;

        //Button areas
        Rectangle startRec;
        Rectangle optionRec;
        Rectangle connectRec;

        public MainMenu(Vector2 sPos, Vector2 oPos, Vector2 cPos){
            startPos = sPos;
            optionsPos = oPos;
            connectPos = cPos;
            buttonResult = "";
        }


        public void load(ContentManager content)
        {
            startTxt = content.Load<Texture2D>("Player\\rat");
            optionsTxt = content.Load<Texture2D>("Player\\rat");
            connectTxt = content.Load<Texture2D>("Player\\rat");
        }

        //Add all the buttons to main's arraylist of buttons
        public void generateMenu()
        {
            main.addButton(startTxt, startPos, Color.Blue);
            main.addButton(optionsTxt, optionsPos, Color.Green);
            main.addButton(connectTxt, connectPos, Color.Purple);
        }

        //not sure what needs to bu updated with menu unless we want fancy stuff
        //also contains logic for commands
        public void Update()
        {
            MouseState mState = Mouse.GetState();
            /*
            if (mState.LeftButton == ButtonState.Pressed)
            {
                if(mState.X.)
            }
            */
        }

        public void Draw(SpriteBatch sb)
        {
            main.Draw(sb);
        }

    }
}