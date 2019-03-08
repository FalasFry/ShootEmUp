using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootEmUp_1._0
{
    public class Enemy
    {
        Texture2D myEnemyTexture;
        Vector2 myPosition;
        Rectangle myRectangle;
        float myRotation;
        float myScale;
        Vector2 myOffset;
        float myEnemyType;
        float mySpeed;
        Vector2 myDir;

        public Enemy(Texture2D aTexture, float aScale, Vector2 aPosition, float anEnemyType)
        {
            myEnemyTexture = aTexture;
            myPosition = aPosition;
            myScale = aScale;
            myEnemyType = anEnemyType;

            myRectangle = new Rectangle(0, 0, myEnemyTexture.Width, myEnemyTexture.Height);
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(myEnemyTexture, myPosition, null, Color.White, myRotation, myOffset, 1f, SpriteEffects.None, 1);
        }

        public void Update(GameTime aGameTime)
        {
            myPosition += (myDir * mySpeed);
            myRectangle.Location = (myPosition - (myRectangle.Size.ToVector2() * 0.5f)).ToPoint();
        }
    }
}
