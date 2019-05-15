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
        public float mySlowerMovements;
        public Vector2 myStartPos;
        public Random myRng;
        public Vector2 myBulletSpawn;

        public List<Texture2D> myTexturesList;

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
            mySlowerMovements = -1f * SkillTree.mySlowerEnemiesMult * 0.15f;

            myTexturesList = new List<Texture2D>()
            {
                GameState.myEnemyTexture,
                GameState.myEnemyTexture2,
            };
        }

        public void StayAlive()
        {
            if (myHealth <= 0)
            {
                if(this is EnemyBoss)
                {
                    GameState.myScore += 5;
                    GameState.myBossTimer = myRng.Next(15,20);
                    SkillTree.myPointMeter -= 5;
                }
                else
                {
                    GameState.myScore++;
                    SkillTree.myPointMeter--;
                }

                myRemove = true;
            }
        }

        public void Collision()
        {
            for (int i = 0; i < GameState.myGameObjects.Count; i++)
            {
                if (GameState.myGameObjects[i] is Player)
                {
                    if (GameState.myGameObjects[i].myRectangle.Intersects(myRectangle))
                    {
                        (GameState.myGameObjects[i] as Player).myHp--;
                        myRemove = true;
                    }
                }
            }
        }

        public void EnemyShoot()
        {
            GameState.myGameObjects.Add(new Bullet(7, new Vector2(0, -1), myBulletTexture, myPosition + myBulletSpawn, 2, myBulletColor));
            myAttackTimer = myStartAttackTimer;
        }

        public void TypeTwoMove(Vector2 aPos, float aSpeed)
        {
            if (myPosition.X > 700 - myTexture.Width || myPosition.X > aPos.X + 100)
            {
                myDir.X = aSpeed * -1;
            }
            if (myPosition.X < 0 || myPosition.X < aPos.X - 100)
            {
                myDir.X = aSpeed;
            }
        }


    }
}
