using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace ShootEmUp_1._0
{

    public class Game1 : Game
    {
        GraphicsDeviceManager myGraphics;
        SpriteBatch mySpriteBatch;

        MenuState myMenu;
        public States myCurState;
        Stack<States> myStateStack;
        public static Color myColor = Color.Black;


        public Game1()
        {
            myGraphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            myStateStack = new Stack<States>();
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            base.Initialize();
            myMenu = new MenuState(this, GraphicsDevice, Content, myGraphics);
            myCurState = myMenu;
            myStateStack.Push(myMenu);
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            mySpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }


        protected override void UnloadContent()
        {
        }


        protected override void Update(GameTime aGameTime)
        {
            if(myStateStack.Peek().Update(aGameTime) == false)
            {
                myStateStack.Pop();
            }
            // TODO: Add your update logic here

            base.Update(aGameTime);
        }


        protected override void Draw(GameTime aGameTime)
        {
            GraphicsDevice.Clear(myColor);

            myStateStack.Peek().Draw(aGameTime, mySpriteBatch);
            // TODO: Add your drawing code here

            base.Draw(aGameTime);
        }

        public void PopStack()
        {
            myStateStack.Pop();
        }

        public void ChangeState(States aState)
        {
            myCurState = aState;
            myStateStack.Push(aState);
        }
    }
}
