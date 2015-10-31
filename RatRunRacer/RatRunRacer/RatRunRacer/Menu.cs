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
    class Menu
    {
       //Stuff needed: constructor
        /*
         * List of buttons
         * List of screens
         * controls
         * instuctions
         */
        //ArrayList for buttons
        ArrayList buttons = new ArrayList();
        
        //not sure what is all needed for an average menu
        public Menu()
        {

        }

        public void addButton(Texture2D buttonTxt, Vector2 buttonPos, Color col)
        {
            Button button = new Button(buttonTxt, buttonPos, col);
            buttons.Add(button);
        }


        public void Draw(SpriteBatch sb)
        {
            //draw all the buttons in the ArrayList for buttons
            foreach(Button button in buttons){
                button.Draw(sb);
            }
        }
    }

}