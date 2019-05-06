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

        public Vector2 accessPosition
        {
            get { return myPosition; }
        }

        Color myColor;

        public BackgroundStars(Texture2D aTexture, Vector2 aNewPos, Vector2 aNewVel, Color aColor)
        {
            myTexture = aTexture;
            myPosition = aNewPos;
            myVelocity = aNewVel;
            myColor = aColor;
        }

        public void Update()
        {
            myPosition += myVelocity;

        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(myTexture, myPosition, myColor);
        }
    }
}
