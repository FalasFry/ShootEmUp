using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShootEmUp_1._0
{
    public class MenuState : States
    {
        KeyboardState myPrevState;

        Texture2D myMenuTexture;
        List<Components> myButtons;
        SaveColors myUnlockables;
        MapEditor myMap;
        SkillTree mySkillTree;

        SpriteFont myButtonFont;
        Texture2D myButtonTexture;
        GraphicsDeviceManager myManager;

        List<string> myStrings;
        List<Vector2> myPositions;
        int mySelected = 0;

        public MenuState(Game1 aGame, GraphicsDevice aGraphicsDevice, ContentManager aContent, GraphicsDeviceManager aManager) : base(aGame, aGraphicsDevice, aContent)
        {
            myManager = aManager;
            myButtonFont = aContent.Load<SpriteFont>("Font");
            myButtonTexture = aContent.Load<Texture2D>("Button");

            myMenuTexture = aContent.Load<Texture2D>("MenuTexture");

            myManager.PreferredBackBufferHeight = 480;
            myManager.PreferredBackBufferWidth = 800;
            myManager.ApplyChanges();

            myUnlockables = new SaveColors();
            myMap = new MapEditor();
            mySkillTree = new SkillTree();

            myStrings = new List<string>()
            {
                "Start Game",
                "Customize",
                "Skill Tree",
                "Quit Game",
            };
            myPositions = new List<Vector2>()
            {
                new Vector2(325, 90),
                new Vector2(325, 190),
                new Vector2(325, 290),
                new Vector2(325, 390),
                new Vector2(325, 490),
            };
        }

        public override void Draw(GameTime aGameTime, SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Begin();
            aSpriteBatch.Draw(myMenuTexture, new Rectangle(0, 0, 800, 480), Color.White);

            for (int i = 0; i < myStrings.Count; i++)
            {
                if (mySelected != i)
                {
                    aSpriteBatch.DrawString(myButtonFont, myStrings[i], myPositions[i], Color.White);
                }
                else if (mySelected == i)
                {
                    aSpriteBatch.DrawString(myButtonFont, myStrings[i], myPositions[i], Color.Yellow);
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

        void Selection()
        {
            KeyboardState tempKeys = Keyboard.GetState();

            if (tempKeys.IsKeyDown(Keys.Enter) && myPrevState.IsKeyUp(Keys.Enter))
            {
                if(mySelected == 0)
                {
                    myGame.ChangeState(new GameState(myGame, myGraphDevice, myContentManager, myManager));
                }
                if (mySelected == 1)
                {
                    myGame.ChangeState(new CustomizeState(myGame, myGraphDevice, myContentManager));
                }
                if (mySelected == 2)
                {
                    myGame.ChangeState(new SkillTreeState(myGame, myGraphDevice, myContentManager));
                }
                if (mySelected == 3)
                {
                    SaveColors.End();
                    MapEditor.End();
                    SkillTree.Update();
                    myGame.Exit();
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
