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

        public abstract void Draw(GameTime aGameTime, SpriteBatch aSpriteBatch);

        public abstract bool Update(GameTime aGameTime);

        public States(Game1 aGame, GraphicsDevice aGraphicsDevice, ContentManager aContent)
        {
            myGame = aGame;
            myGraphDevice = aGraphicsDevice;
            myContentManager = aContent;
        }
    }
}
