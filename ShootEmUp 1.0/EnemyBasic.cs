using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootEmUp_1._0
{
    public abstract class EnemyBasic
    {

        public Texture2D myEnemyTexture;
        public Vector2 myPosition;
        public Rectangle myRectangle;
        public Vector2 myOffset;
        public Vector2 myDir;
        public Vector2 myStartPos;
        public Random myRng;
        public Vector2 myBulletSpawn;

        public int myMove;
        public float myEnemyType;
        public float mySpeed;
        public float myDeltaTime;
        public float myRotation;
        public float myScale;

        public EnemyBasic(Texture2D aTexture)
        {
            myEnemyTexture = aTexture;
            myRectangle = new Rectangle(0, 0, myEnemyTexture.Width, myEnemyTexture.Height);
            myBulletSpawn = new Vector2(myEnemyTexture.Bounds.Size.X * 0.5f, 0);
        }

        public abstract void Update(GameTime aGameTime);
        public abstract void Draw(SpriteBatch aSpriteBatch);
    }
}
