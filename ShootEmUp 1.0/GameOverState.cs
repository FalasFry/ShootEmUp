using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ShootEmUp_1._0
{
    class GameOverState : States
    {
        SpriteFont myFont;
        float myPoints;

        Texture2D myButtonTexture;
        GraphicsDeviceManager myManager;

        List<Button> myButtons;

        public GameOverState(Game1 aGame, GraphicsDevice aGraphicsDevice, ContentManager aContent, float aScore, GraphicsDeviceManager aManager) : base(aGame, aGraphicsDevice, aContent)
        {
            myPoints = aScore;
            myFont = aContent.Load<SpriteFont>("Font");
            myButtonTexture = aContent.Load<Texture2D>("Button");
            myManager = aManager;

            Button myRestartButton = new Button(myFont, myButtonTexture)
            {
                AccessText = "Restart Game",
                AccessPos = new Vector2(300, 280),
            };
            myRestartButton.Click += myRestartButton_Click1;
            
            Button myQuitButton = new Button(myFont, myButtonTexture)
            {
                AccessText = "Quit Game",
                AccessPos = new Vector2(300, 340),
            };
            myQuitButton.Click += MyQuitButton_Click;

            myButtons = new List<Button>()
            {
                myRestartButton,
                myQuitButton,
            };
            
        }

        private void MyQuitButton_Click(object sender, EventArgs e)
        {
            SaveColors.End();
            MapEditor.End();
            SkillTree.End();
            myGame.Exit();
        }

        private void myRestartButton_Click1(object sender, EventArgs e)
        {
            myManager.PreferredBackBufferHeight = 480;
            myManager.PreferredBackBufferWidth = 800;
            myManager.ApplyChanges();

            myGame.PopStack();
            myGame.PopStack();
        }

        public override void Draw(GameTime aGameTime, SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Begin();

            aSpriteBatch.DrawString(myFont, "Game Over", new Vector2(300, 200), Color.White);
            aSpriteBatch.DrawString(myFont, "You Got A Score Of " +myPoints +" Points" , new Vector2(190, 240), Color.White);

            foreach(Button button in myButtons)
            {
                button.Draw(aGameTime, aSpriteBatch);
            }

            aSpriteBatch.End();
        }

        public override bool Update(GameTime aGameTime)
        {
            foreach(Button button in myButtons)
            {
                button.Update(aGameTime);
            }
            return true;
        }
    }
}
