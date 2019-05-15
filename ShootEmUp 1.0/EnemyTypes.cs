﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ShootEmUp_1._0
{
    class EnemyEasy : EnemyBase
    {
        public EnemyEasy(Texture2D aTexture, Vector2 aPosition)
        {
            myRotation = 0;
            myPosition = aPosition;
            mySpeed = 5 + mySlowerMovements;
            myStartPos = myPosition;
            myDir = new Vector2(0, -1);
            myTexture = aTexture;
            myBulletTexture = GameState.myEnemyBullet;
            myRectangle = new Rectangle(0, 0, myTexture.Width * (int)myScale, myTexture.Height * (int)myScale);
            myBulletSpawn = new Vector2(myRectangle.Width * 0.5f, 0);
            myBulletColor = Color.Cyan;
            myColor = Color.Cyan;
        }

        public override void Update(GameTime aGameTime)
        {
            Collision();
            myPosition += (myDir * mySpeed);
            myRectangle.Location = myPosition.ToPoint();
            StayAlive();
            myAttackTimer -= GameState.myDeltaTime;
            Animation(myTexturesList);

            if (myAttackTimer <= 0)
            {
                EnemyShoot();
            }
        }
    }

    class EnemyMoving : EnemyBase
    {
        public EnemyMoving(Texture2D aTexture, Vector2 aPosition)
        {
            myPosition = aPosition;
            mySpeed = 5 + mySlowerMovements;
            myBulletColor = Color.Cyan;
            myStartPos = myPosition;
            myBulletTexture = GameState.myEnemyBullet;
            myTexture = aTexture;
            myRectangle = new Rectangle(0, 0, myTexture.Width * (int)myScale, myTexture.Height * (int)myScale);
            myBulletSpawn = new Vector2(myRectangle.Width * 0.5f, 0);
            myColor = Color.Red;

            int tempRng = myRng.Next(1, 3);
            myDir = new Vector2(tempRng, -1);
        }

        public override void Update(GameTime aGameTime)
        {
            TypeTwoMove(myStartPos, 1);
            Collision();

            myAttackTimer -= GameState.myDeltaTime;
            if (myAttackTimer <= 0)
            {
                EnemyShoot();
            }
            StayAlive();
            myPosition += (myDir * mySpeed);
            myRectangle.Location = myPosition.ToPoint();
        }
    }
    class EnemyBoss : EnemyBase
    {
        int myMoveDirX;
        float myBossAS = 0.3f;

        public EnemyBoss(Texture2D aTexture, float aShootWay)
        {
            myShootStyle = aShootWay;
            myPosition = new Vector2(270, 900);
            myHealth = 5;
            mySpeed = 3 + mySlowerMovements;
            myScale = 2;
            myDir = new Vector2(0, -1);
            myTexture = aTexture;
            myBulletTexture = GameState.myEnemyBullet;
            myRectangle = new Rectangle(0, 0, myTexture.Width * (int)myScale, myTexture.Height * (int)myScale);
            myBulletSpawn = new Vector2(myRectangle.Width * 0.5f, 0);

            int tempRng = myRng.Next(1, 3);

            if (tempRng == 1)
            {
                myMoveDirX = -1;
            }
            else
            {
                myMoveDirX = 1;
            }
        }

        public override void Update(GameTime aGameTime)
        {
            myBossAS -= GameState.myDeltaTime;
            Animation(myTexturesList);

            if (myBossAS <= 0)
            {
                BossShoot();
            }

            if (myPosition.Y <= 600 && myDir.X == 0)
            {
                myDir.X = myMoveDirX;
                myDir.Y = 0;
            }

            if (myPosition.X > 600 - myTexture.Width)
            {
                myDir.X = -1;
            }
            else if (myPosition.X < 100)
            {
                myDir.X = 1;
            }
            StayAlive();
            myPosition += (myDir * mySpeed);
            myRectangle.Location = myPosition.ToPoint();
        }

        public void BossShoot()
        {
            float tempBossAS = 0.3f;

            if (myShootStyle == 1)
            {
                GameState.myGameObjects.Add(new Bullet(7, new Vector2(0, -1), myBulletTexture, myPosition + myBulletSpawn, 2, Color.Aqua));
                GameState.myGameObjects.Add(new Bullet(7, new Vector2(1, -1), myBulletTexture, myPosition + myBulletSpawn, 2, Color.Aqua));
                GameState.myGameObjects.Add(new Bullet(7, new Vector2(-1, -1), myBulletTexture, myPosition + myBulletSpawn, 2, Color.Aqua));
            }
            else if (myShootStyle == 2)
            {
                GameState.myGameObjects.Add(new Bullet(7, new Vector2(0, -1), myBulletTexture, myPosition + myBulletSpawn + new Vector2(50, 0), 2, Color.Aqua));
                GameState.myGameObjects.Add(new Bullet(7, new Vector2(0, -1), myBulletTexture, myPosition + myBulletSpawn + new Vector2(-50, 0), 2, Color.Aqua));
                GameState.myGameObjects.Add(new Bullet(7, new Vector2(0, -1), myBulletTexture, myPosition + myBulletSpawn, 2, Color.Aqua));
            }
            
            myBossAS = tempBossAS;
        }
    }

    class EnemySmart : EnemyBase
    {
        Vector2 myShootDir;
        float mySmartAS;
        float mySmartStartAS = 0.3f;
        public EnemySmart(Texture2D aTexture, Vector2 aPosition)
        {
            myRotation = 0;
            myPosition = aPosition;
            mySpeed = 5 + mySlowerMovements;
            myStartPos = myPosition;
            myTexture = aTexture;
            myBulletTexture = GameState.myEnemyBullet;
            myRectangle = new Rectangle(0, 0, myTexture.Width * (int)myScale, myTexture.Height * (int)myScale);
            myBulletSpawn = new Vector2(myRectangle.Width * 0.5f, 0);
            myBulletColor = Color.Purple;
            myColor = Color.Orange;
            myOffset = ((myTexture.Bounds.Size.ToVector2() * 0.5f));

            int tempRng = myRng.Next(1, 3);
            myDir = new Vector2(tempRng, -1);
        }

        public override void Update(GameTime aGameTime)
        {
            Animation(myTexturesList);
            StayAlive();
            Collision();
            TypeTwoMove(myStartPos, 1.5f);

            myPosition += (myDir * mySpeed);
            myRectangle.Location = myPosition.ToPoint();
            mySmartAS -= GameState.myDeltaTime;

            if (mySmartAS<= 0)
            {
                myShootDir = GameState.myPlayer.myPosition - (myPosition + myOffset);
                myShootDir.Normalize();
                SmartBullets(myShootDir);
            }
        }

        void SmartBullets(Vector2 aShootDir)
        {
            GameState.myGameObjects.Add(new Bullet(7, aShootDir, myBulletTexture, myPosition + myBulletSpawn, 2, myBulletColor));
            mySmartAS = mySmartStartAS;
        }
    }
}
