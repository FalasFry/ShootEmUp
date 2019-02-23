using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ShootEmUp_1._0
{
    public abstract class States
    {
        protected ContentManager myContentManager;

        protected GraphicsDevice myGraphDevice;

        protected Game1 myGame;

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract bool Update(GameTime gameTime);

        public States(Game1 Game, GraphicsDevice graphicsDevice, ContentManager content)
        {
            myGame = Game;
            myGraphDevice = graphicsDevice;
            myContentManager = content;
        }
    }
}
