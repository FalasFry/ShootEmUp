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
        public GameOverState(Game1 aGame, GraphicsDevice aGraphicsDevice, ContentManager aContent) : base(aGame, aGraphicsDevice, aContent)
        {

        }

        public override void Draw(GameTime aGameTime, SpriteBatch aSpriteBatch)
        {

        }

        public override bool Update(GameTime aGameTime)
        {

            return true;
        }
    }
}
