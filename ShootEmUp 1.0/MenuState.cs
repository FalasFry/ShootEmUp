﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ShootEmUp_1._0
{
    public class MenuState : States
    {
        Texture2D myMenuTexture;
        List<Components> myButtons;
        SaveColors myUnlockables;

        SpriteFont myButtonFont;
        Texture2D myButtonTexture;
        GraphicsDeviceManager myManager;

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

            Button myStartButton = new Button(myButtonFont, myButtonTexture)
            {
                AccessText = "Start Game",
                AccessPos = new Vector2(325, 190),
            };
            myStartButton.Click += StartButton_Click1;

            Button myCustomButtom = new Button(myButtonFont, myButtonTexture)
            {
                AccessText = "Customize",
                AccessPos = new Vector2(325, 390),
            };
            myCustomButtom.Click += MyCustomButtom_Click;

            Button myQuitButton = new Button(myButtonFont, myButtonTexture)
            {
                AccessText = "Quit",
                AccessPos = new Vector2(325, 290),
            };
            myQuitButton.Click += QuitButton_Click;

            myButtons = new List<Components>()
            {
                myStartButton,
                myCustomButtom,
                myQuitButton,
            };
        }

        private void MyCustomButtom_Click(object sender, EventArgs e)
        {
            myGame.ChangeState(new CustomizeState(myGame, myGraphDevice, myContentManager));
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            SaveColors.End();
            myGame.Exit();
        }

        private void StartButton_Click1(object sender, EventArgs e)
        {
            myGame.ChangeState(new GameState(myGame, myGraphDevice, myContentManager, myManager));
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
