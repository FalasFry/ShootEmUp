using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootEmUp_1._0
{
    abstract class EnemyBase : GameObject
    {

        public Vector2 myStartPos;
        public Random myRng;
        public Vector2 myBulletSpawn;

        public float myHealth;
        public float myShootStyle;
        public Color myBulletColor = Color.White;
        public float myAttackTimer = 0;
        public float myStartAttackTimer = 0.5f;
        public Texture2D myBulletTexture;

        public EnemyBase()
        {
            myHealth = 1;
            myScale = 1;
            myRng = new Random();
        }

    }
}
