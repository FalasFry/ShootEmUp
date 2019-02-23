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
    class GameState : States
    {
        float myAttackTimer = 10;
        float myMoveTimer = 15;
        float mySpeed = 1;
        public float myScore;
        float myHealth = 10;

        Player myPlayer;
        Texture2D myBullet;
        Random myRng = new Random();
        SpriteFont myFont;
        Texture2D myPowerupsTexture;
        Texture2D myEnemyTexture;
        Texture2D myPlayerTexture;

        public GameState(Game1 Game, GraphicsDevice graphicsDevice, ContentManager content) : base(Game, graphicsDevice, content)
        {
            myEnemyTexture = content.Load<Texture2D>("enemy");
            myPowerupsTexture = content.Load<Texture2D>("ball");
            myPlayerTexture = content.Load<Texture2D>("player");

            myPlayer = new Player(myGame, myPlayerTexture)
            {
                myHp = myHealth,
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            myPlayer.Draw(spriteBatch);

            spriteBatch.End();
        }

        public override bool Update(GameTime aGameTime)
        {
            myPlayer.Update(aGameTime);
            return true;
        }
    }
}
