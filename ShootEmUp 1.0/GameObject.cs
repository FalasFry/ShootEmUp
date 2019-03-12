using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootEmUp_1._0
{
    abstract class GameObject
    {
        public float mySpeed, myRotation;
        public Vector2 myDir, myPosition, myOffset;
        public Texture2D myTexture;
        public Rectangle myRectangle;
        public float myDamage = 1;
        public Color myColor = Color.White;
        public float myScale = 1;
        public bool myRemove = false;

        public abstract void Update(GameTime aGameTime);

        public void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(myTexture, myPosition, null, myColor, myRotation, myOffset, myScale, SpriteEffects.None, 1);
        }



    }
}
