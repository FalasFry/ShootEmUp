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

        public List<Texture2D> myTexturesList;

        public float myHealth;
        public float myShootStyle;
        public Color myBulletColor = Color.White;
        public float myAttackTimer = 0;
        public float myStartAttackTimer = 0.5f;
        public Texture2D myBulletTexture;
        float myTimer = 1;

        public EnemyBase()
        {
            myHealth = 1;
            myScale = 1;
            myRng = new Random();

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
                    GameState.myBossTimer = 5;
                }
                else
                {
                    GameState.myScore++;
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

        public void Animation(List<Texture2D> aList, int anI)
        {
            bool tempBool = false;

            if (anI < aList.Count)
            {
                tempBool = true;
            }

            if(tempBool)
            {
                myTimer -= GameState.myDeltaTime;

                if(myTimer <= 0 && anI < aList.Count - 1)
                {
                    myTimer = 1;
                    if (myTexture == myTexturesList[0])
                    {
                        myTexture = myTexturesList[1];
                    }
                    else if (myTexture == myTexturesList[1])
                    {
                        myTexture = myTexturesList[0];
                    }

                    //for (int i = 0; i < myTexturesList.Count; i++)
                    //{
                    //    if(myTexture == myTexturesList[i])
                    //    {
                    //        if (i < myTexturesList.Count - 1)
                    //        {
                    //            myTexture = myTexturesList[i + 1];
                    //        }
                    //        else if (i >= myTexturesList.Count - 1)
                    //        {
                    //            myTexture = myTexturesList[0];
                    //        }
                    //    }
                    //}
                }
            }
        }
    }
}
