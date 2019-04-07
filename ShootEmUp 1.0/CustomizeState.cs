using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ShootEmUp_1._0
{
    class CustomizeState : States
    {
        Texture2D myBlue, myPink, myGreen, myBasic;

        List<Texture2D> myTextures;
        List<bool> myBools;

        static public Texture2D myTexture;
        KeyboardState myPrevState;
        Vector2 myPos;
        SpriteFont myFont;
        bool myWait = false;
        float myTimer = 0.5f;
        float myDeltaTime;

        bool myBaseBool = true;
        bool myFirst = true;
        bool mySecond;
        bool myThird;

        int mySelected = 0;
        
        public CustomizeState(Game1 aGame, GraphicsDevice aGraphicsDevice, ContentManager aContent) : base(aGame, aGraphicsDevice, aContent)
        {
            myBlue = aContent.Load<Texture2D>("PlayerShipBlue");
            myPink = aContent.Load<Texture2D>("PlayerShipPink");
            myGreen = aContent.Load<Texture2D>("PlayerShipGreen");
            myBasic = aContent.Load<Texture2D>("PlayerShip");
            myFont = aContent.Load<SpriteFont>("Font");

            myTexture = myBasic;
            myGraphDevice = aGraphicsDevice;
            myPos = new Vector2(325, 150);
            myTextures = new List<Texture2D>()
            {
                myBasic,
                myBlue,
                myPink,
                myGreen
            };

            myBools = new List<bool>()
            {
                myBaseBool,
                myFirst,
                mySecond,
                myThird
            };
        }

        public override void Draw(GameTime aGameTime, SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Begin();
            aSpriteBatch.Draw(myTexture, myPos, Color.White);

            for (int i = 0; i < myBools.Count; i++)
            {
                if(mySelected == i)
                {
                    aSpriteBatch.DrawString(myFont, "Unlocked " + myBools[i], myPos + new Vector2(-30, 200), Color.White);
                }
            }
            aSpriteBatch.DrawString(myFont, "Press ESCAPE For Back And ENTER To Select", myPos + new Vector2(-200, -100), Color.White);
            aSpriteBatch.End();
        }

        public override bool Update(GameTime aGameTime)
        {
            KeyboardState tempKeys = Keyboard.GetState();
            MoveSelection();
            
            Selection();

            if (tempKeys.IsKeyDown(Keys.Escape))
            {
                myTexture = myBasic;
                myGame.PopStack();
            }

            if(tempKeys.IsKeyDown(Keys.Enter))
            {
                if(mySelected == 1 && myFirst)
                {
                    myGame.PopStack();
                }
                if (mySelected == 2 && mySecond)
                {
                    myGame.PopStack();
                }
                if(mySelected == 3 && myThird)
                {
                    myGame.PopStack();
                }

            }

            myPrevState = tempKeys;
            return true;
        }

        public void Selection()
        {
            for (int i = 0; i < myTextures.Count; i++)
            {
                if(mySelected == i)
                {
                    myTexture = myTextures[i];
                    return;
                }
            }
        }

        void MoveSelection()
        {
            KeyboardState tempKeys = Keyboard.GetState();

            if (tempKeys.IsKeyDown(Keys.Left) && mySelected > 0 && myPrevState.IsKeyUp(Keys.Left))
            {
                mySelected--;
            }

            if (tempKeys.IsKeyDown(Keys.Right) && mySelected < myTextures.Count - 1 && myPrevState.IsKeyUp(Keys.Right))
            {
                mySelected++;
            }
        }
    }
}
