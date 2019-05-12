using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ShootEmUp_1._0
{
    class BackgroundStars
    {
        Texture2D myTexture;
        Vector2 myPosition;
        Vector2 myVelocity;
        float myScale;

        public Vector2 accessPosition
        {
            get { return myPosition; }
        }

        Color myColor;

        public BackgroundStars(Texture2D aTexture, Vector2 aNewPos, Vector2 aNewVel, Color aColor, float aScale)
        {
            myTexture = aTexture;
            myPosition = aNewPos;
            myVelocity = aNewVel;
            myColor = aColor;
            myScale = aScale;
        }

        public void Update()
        {
            myPosition += myVelocity;
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(myTexture, myPosition, null, myColor, 0, new Vector2(), myScale, SpriteEffects.None, 1);
        }
    }
}
