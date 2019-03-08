using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootEmUp_1._0
{
    class Enemy
    {
        Texture2D myEnemyTexture;
        Vector2 myPosition;
        Rectangle myRectangle;
        float myRotation;
        float myScale;
        Vector2 myOffset;

        public Enemy(Texture2D aTexture, float aScale, Vector2 aPosition)
        {
            myEnemyTexture = aTexture;
            myPosition = aPosition;
            myScale = aScale;

            myRectangle = new Rectangle(0, 0, myEnemyTexture.Width, myEnemyTexture.Height);
        }

        public void Draw(SpriteBatch aSpriteBatch, GameTime aGameTime)
        {
            aSpriteBatch.Draw(myEnemyTexture, myPosition, null, Color.White, myRotation, myOffset, 1f, SpriteEffects.None, 1);
        }

        public void Update(GameTime aGameTime)
        {

        }
    }
}
