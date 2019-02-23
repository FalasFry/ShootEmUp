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
    class MenuState : States
    {
        Texture2D myMenuTexture;
        List<Components> myButtons;

        SpriteFont myButtonFont;
        Texture2D myButtonTexture;

        public MenuState(Game1 aGame, GraphicsDevice aGraphicsDevice, ContentManager aContent) : base(aGame, aGraphicsDevice, aContent)
        {
            myButtonFont = aContent.Load<SpriteFont>("Font");
            myButtonTexture = aContent.Load<Texture2D>("Button");

            myMenuTexture = aContent.Load<Texture2D>("MenuTexture");

            Button startButton = new Button(myButtonFont, myButtonTexture)
            {
                AccessText = "Start Game",
                AccessPos = new Vector2(325, 190),
            };
            startButton.Click += StartButton_Click1;

            Button quitButton = new Button(myButtonFont, myButtonTexture)
            {
                AccessText = "Quit",
                AccessPos = new Vector2(325, 290),
            };
            quitButton.Click += QuitButton_Click;

            myButtons = new List<Components>()
            {
                startButton,
                quitButton,
            };
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            myGame.Exit();
        }

        private void StartButton_Click1(object sender, EventArgs e)
        {
            myGame.ChangeState(new GameState(myGame, myGraphDevice, myContentManager));
        }

        public override void Draw(GameTime aGameTime, SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Begin();
            aSpriteBatch.Draw(myMenuTexture, new Rectangle(0, 0, 800, 480), Color.White);

            foreach(Button component in myButtons)
            {
                component.Draw(aGameTime, aSpriteBatch);
            }
            aSpriteBatch.End();
        }

        public override bool Update(GameTime aGameTime)
        {
            for (int i = 0; i < myButtons.Count; i++)
            {
                myButtons[i].Update(aGameTime);
            }
            return true;
        }
    }
}
