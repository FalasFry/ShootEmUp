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

        int mySelected = 0;

        List<string> myStrings;

        List<Vector2> myPosisions;
        
        public SkillTreeState(Game1 aGame, GraphicsDevice aGraphicsDevice, ContentManager aContent) : base(aGame, aGraphicsDevice, aContent)
        {
            myFont = aContent.Load<SpriteFont>("Font");

            myGraphDevice = aGraphicsDevice;

            myStrings = new List<string>()
            {
                "Unlock Super",
                "Upgrade Speed",
                "Upgrade Firerate",
                "Slower Enemies",
                "Upgrade Health",
            };

            myPosisions = new List<Vector2>()
            {
                new Vector2(25, 25),
                new Vector2(25, 75),
                new Vector2(25, 125),
                new Vector2(25, 175),
                new Vector2(25, 225),
            };

        }

        public override void Draw(GameTime aGameTime, SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Begin();

            for (int i = 0; i < myStrings.Count(); i++)
            {
                if (mySelected != i)
                {
                    aSpriteBatch.DrawString(myFont, myStrings[i], myPosisions[i], Color.White);
                }
                else if(mySelected == i)
                {
                    aSpriteBatch.DrawString(myFont, myStrings[i], myPosisions[i], Color.Yellow);
                }
            }

            aSpriteBatch.End();
        }

        public override bool Update(GameTime aGameTime)
        {
            MoveSelection();
            Selection();

            return true;
        }

        public void Selection()
        {
            KeyboardState tempKeys = Keyboard.GetState();
            if (tempKeys.IsKeyDown(Keys.Escape))
            {
                myGame.PopStack();
            }

            if (tempKeys.IsKeyDown(Keys.Enter))
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
    }
}
