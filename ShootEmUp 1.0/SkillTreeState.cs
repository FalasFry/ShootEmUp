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
    class SkillTreeState : States
    {
        KeyboardState myPrevState;
        SpriteFont myFont;
        Texture2D myButtonTexture;

        int mySelected = 0;
        int myPoints;

        List<string> myStrings;
        List<string> myValues;

        List<Vector2> myPos;
        List<Vector2> mySecondPos;
        List<Components> myButtons;
        
        public SkillTreeState(Game1 aGame, GraphicsDevice aGraphicsDevice, ContentManager aContent) : base(aGame, aGraphicsDevice, aContent)
        {
            myFont = aContent.Load<SpriteFont>("Font");
            myButtonTexture = aContent.Load<Texture2D>("button");
            myGraphDevice = aGraphicsDevice;
            myPoints = SkillTree.myPointsToSpend;

            myStrings = new List<string>()
            {
                "Unlock Super",
                "Upgrade Speed",
                "Upgrade Firerate",
                "Slower Enemies",
                "Upgrade Health",
            };
            myValues = new List<string>()
            {
                Convert.ToString(SkillTree.myUnlockSupers),
                Convert.ToString(SkillTree.mySpeedMult),
                Convert.ToString(SkillTree.myFirerateMult),
                Convert.ToString(SkillTree.mySlowerEnemiesMult),
                Convert.ToString(SkillTree.myHealthUpgrade),
            };
            mySecondPos = new List<Vector2>()
            {
                new Vector2(500, 25),
                new Vector2(500, 75),
                new Vector2(500, 125),
                new Vector2(500, 175),
                new Vector2(500, 225),
            };
            myPos = new List<Vector2>()
            {
                new Vector2(25, 25),
                new Vector2(25, 75),
                new Vector2(25, 125),
                new Vector2(25, 175),
                new Vector2(25, 225),
            };

            #region ResetButton
            Button tempResetButton = new Button(myFont, myButtonTexture)
            {
                AccessText = "RESET",
                AccessPos = new Vector2(400 - (myButtonTexture.Width / 2), 450 - myButtonTexture.Height),
            };
            tempResetButton.Click += TempResetButton_Click;
            myButtons = new List<Components>()
            {
                //tempResetButton,
            };
            #endregion
        }

        private void TempResetButton_Click(object sender, EventArgs e)
        {
            SkillTree.Reset();
            Reset();
        }

        public override void Draw(GameTime aGameTime, SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Begin();

            for (int i = 0; i < myStrings.Count; i++)
            {
                if (mySelected != i)
                {
                    aSpriteBatch.DrawString(myFont, myStrings[i], myPos[i], Color.White);
                }
                else if(mySelected == i)
                {
                    aSpriteBatch.DrawString(myFont, myStrings[i], myPos[i], Color.Yellow);
                }
            }

            aSpriteBatch.DrawString(myFont, "Points: " + myPoints, new Vector2(410 - (myButtonTexture.Width / 2), 400 - myButtonTexture.Height) ,Color.White);

            for (int i = 0; i < myValues.Count; i++)
            {
                if (myValues[i] == "10" || myValues[i] == "True")
                {
                    string tempString = "Max";
                    aSpriteBatch.DrawString(myFont, "Currently: " + tempString, mySecondPos[i], Color.White);
                }
                else
                {
                    aSpriteBatch.DrawString(myFont, "Currently: " + myValues[i], mySecondPos[i], Color.White);
                }
            }

            for (int i = 0; i < myButtons.Count; i++)
            {
                myButtons[i].Draw(aGameTime, aSpriteBatch);
            }

            aSpriteBatch.End();
        }

        public override bool Update(GameTime aGameTime)
        {
            MoveSelection();
            Selection();

            for (int i = 0; i < myButtons.Count; i++)
            {
                myButtons[i].Update(aGameTime);
            }

            return true;
        }

        public void Selection()
        {
            KeyboardState tempKeys = Keyboard.GetState();

            if (tempKeys.IsKeyDown(Keys.Escape))
            {
                SkillTree.Update();
                myGame.PopStack();
            }

            if (tempKeys.IsKeyDown(Keys.Enter) && myPrevState.IsKeyUp(Keys.Enter) && myPoints > 0)
            {
                if (mySelected == 0)
                {
                    if (!SkillTree.myUnlockSupers)
                    {
                        SkillTree.myUnlockSupers = true;
                    }
                }
                if (mySelected == 1)
                {
                    if (SkillTree.mySpeedMult < 10)
                    {
                        SkillTree.mySpeedMult++;
                    }
                }
                if (mySelected == 2)
                {
                    if (SkillTree.myFirerateMult < 10)
                    {
                        SkillTree.myFirerateMult++;
                    }
                }
                if (mySelected == 3)
                {
                    if (SkillTree.mySlowerEnemiesMult < 10)
                    {
                        SkillTree.mySlowerEnemiesMult++;
                    }
                }
                if (mySelected == 4)
                {
                    if (SkillTree.myHealthUpgrade < 10)
                    {
                        SkillTree.myHealthUpgrade++;
                    }
                }
                SkillTree.myPointsToSpend--;
                Reset();
            }
            myPrevState = tempKeys;
        }

        void MoveSelection()
        {
            KeyboardState tempKeys = Keyboard.GetState();

            if (tempKeys.IsKeyDown(Keys.Up) && mySelected > 0 && myPrevState.IsKeyUp(Keys.Up))
            {
                mySelected--;
            }

            if (tempKeys.IsKeyDown(Keys.Down) && mySelected < myStrings.Count() - 1 && myPrevState.IsKeyUp(Keys.Down))
            {
                mySelected++;
            }
        }

        void Reset()
        {
            myValues = new List<string>()
            {
                Convert.ToString(SkillTree.myUnlockSupers),
                Convert.ToString(SkillTree.mySpeedMult),
                Convert.ToString(SkillTree.myFirerateMult),
                Convert.ToString(SkillTree.mySlowerEnemiesMult),
                Convert.ToString(SkillTree.myHealthUpgrade),
            };

            myPoints = SkillTree.myPointsToSpend;
        }
    }
}
