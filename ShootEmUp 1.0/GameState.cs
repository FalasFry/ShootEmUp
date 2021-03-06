﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShootEmUp_1._0
{
    class GameState : States
    {
        #region Classes

        public static Texture2D myEnemyTexture;
        public static Texture2D myEnemyTexture2;
        public static Texture2D myEnemyCharge1;
        public static Texture2D myEnemyCharge2;
        public static Texture2D myEnemyCharge3;
        public static Texture2D myEnemyLazer;
        public static Texture2D myBullet;
        public static Texture2D myEnemyBullet;
        public static List<GameObject> myGameObjects;
        public static Player myPlayer;
        public static Texture2D myPlayerTexture;
        Texture2D myPowerupsTexture;
        Texture2D myWallTexture;
        Texture2D myCharachterPuTexture;
        Texture2D myStarsTexture;
        Random myRng;
        SpriteFont myFont;
        ParticleGenerator myStars;
        TimeSpan myPreviousSpawnTime;
        TimeSpan myEnemySpawnTime;
        GraphicsDeviceManager myGraphics;

        #endregion

        #region Variables

        public static float myScore;
        public static float myDeltaTime;
        public static float myDisplayTextTimer;
        public static float myBossTimer;
        public static float myPowerUpCoolDownSeconds;
        public static float mySuperPowerCoolDownSeconds;
        public static string myPowerUp;
        public static int myPowerUpCount = 0;
        public static int myWallsDestroyed = 0;
        public static bool mySuperPowerUpUnlocked = false;
        public static bool myShowText;
        public static bool myUltimateCoolDown;
        public static bool myPowerUpCoolDown;
        float myPowerUpSpawnTime;
        float myCharachterPuSpawnTime;
        float mySpawnWallsTimer;
        bool mySpawnWalls;
        string myRound;

        #endregion

        public GameState(Game1 aGame, GraphicsDevice aGraphicsDevice, ContentManager aContent, GraphicsDeviceManager aManager) : base(aGame, aGraphicsDevice, aContent)
        {
            Reset();

            #region WindowSize
            myGraphics = aManager;
            aManager.PreferredBackBufferHeight = 900;
            aManager.PreferredBackBufferWidth = 700;
            aManager.ApplyChanges();
            #endregion

            #region LoadContent
            myEnemyTexture = aContent.Load<Texture2D>("EnemyShip");
            myEnemyTexture2 = aContent.Load<Texture2D>("EnemyShip2");
            myEnemyCharge1 = aContent.Load<Texture2D>("Charge1");
            myEnemyCharge2 = aContent.Load<Texture2D>("Charge2");
            myEnemyCharge3 = aContent.Load<Texture2D>("Charge3");
            myPowerupsTexture = aContent.Load<Texture2D>("PowerUp");
            myPlayerTexture = CustomizeState.myTexture;
            myBullet = aContent.Load<Texture2D>("BulletPixel");
            myEnemyBullet = aContent.Load<Texture2D>("ball");
            myFont = aContent.Load<SpriteFont>("Font");
            myWallTexture = aContent.Load<Texture2D>("Walls");
            myCharachterPuTexture = aContent.Load<Texture2D>("MarioStar");
            myEnemyLazer = aContent.Load<Texture2D>("Lazer");
            myStarsTexture = aContent.Load<Texture2D>("Star");
            #endregion

            myRng = new Random();
            myStars = new ParticleGenerator(myStarsTexture, aManager.PreferredBackBufferWidth, 100);
            myGameObjects = new List<GameObject>();

            if (myPlayerTexture != null)
            {
                myPlayer = new Player(myPlayerTexture);
            }
            else if (myPlayerTexture == null)
            {
                myPlayer = new Player(aContent.Load<Texture2D>("PlayerShip"));
            }

            myGameObjects.Add(myPlayer);
            mySuperPowerUpUnlocked = SkillTree.myUnlockSupers;
        }

        public override void Draw(GameTime aGameTime, SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Begin();
            myStars.Draw(aSpriteBatch);

            for (int i = 0; i < myGameObjects.Count; i++)
            {
                myGameObjects[i].Draw(aSpriteBatch);
            }

            aSpriteBatch.DrawString(myFont, "HP Left: " + myPlayer.myHp.ToString(), new Vector2(400, 0), Color.White);
            aSpriteBatch.DrawString(myFont, "Score: " + myScore, new Vector2(200, 0), Color.White);

            int tempTextOffsetX = 100;
            int tempTextOffsetY = 50;
            if (myPowerUpCoolDown)
            {
                string tempText = "PowerUp Time Left " + (int)myPowerUpCoolDownSeconds;
                aSpriteBatch.DrawString(myFont, tempText, new Vector2((myGraphics.PreferredBackBufferWidth / 2 - tempTextOffsetX), myGraphics.PreferredBackBufferHeight / 2), Color.White);
            }
            if(myUltimateCoolDown)
            {
                string tempText = "PowerUp Time Left " + (int)mySuperPowerCoolDownSeconds;
                aSpriteBatch.DrawString(myFont, tempText, new Vector2((myGraphics.PreferredBackBufferWidth / 2 - tempTextOffsetX), myGraphics.PreferredBackBufferHeight / 2), Color.White);
            }
            if (myShowText)
            {
                string tempText = "You Got " + myPowerUp;
                aSpriteBatch.DrawString(myFont, tempText, new Vector2((myGraphics.PreferredBackBufferWidth / 2 - tempTextOffsetX), myGraphics.PreferredBackBufferHeight / 2 - tempTextOffsetY), Color.White);
            }

            aSpriteBatch.DrawString(myFont, "Round: " + myRound, new Vector2(myGraphics.PreferredBackBufferWidth / 2 - 40, myGraphics.PreferredBackBufferHeight - 40), Color.White);

            aSpriteBatch.End();
        }

        public override bool Update(GameTime aGameTime)
        {
            myDeltaTime = (float)aGameTime.ElapsedGameTime.TotalSeconds;

            myStars.Update(aGameTime, myGraphDevice);
            OutOfBounds();
            SpawnBoss();
            EnemySpawn(aGameTime);

            if (!myUltimateCoolDown)
            {
                PowerUpSpawn();
            }

            if (mySuperPowerUpUnlocked && !myPowerUpCoolDown)
            {
                SuperUpSpawn();
            }

            for (int i = 0; i < myGameObjects.Count; i++)
            {
                myGameObjects[i].Update(aGameTime);
            }

            if (myPowerUpCoolDown)
            {
                PowerUpTimer(SmallPowerUps.myPowerType, myPlayer.myBaseAttackSpeed, myPlayer.myNormalSpeed);
            }

            if (myUltimateCoolDown)
            {
                SuperPowerUpTimer(myPlayer.myBaseAttackSpeed);
            }

            if(SkillTree.myPointMeter <= 0)
            {
                SkillTree.myPointsToSpend++;
                SkillTree.myPointMeter += 100;
            }

            if (myScore <= 0)
            {
                myScore = 0;
            }

            if (myShowText)
            {
                myDisplayTextTimer -= myDeltaTime;
                if (myDisplayTextTimer <= 0)
                {
                    myShowText = false;
                    myDisplayTextTimer = 2;
                }
            }

            if (myPlayer.myHp <= 0)
            {
                myGame.ChangeState(new GameOverState(myGame, myGraphDevice, myContentManager, myScore, myGraphics));
            }

            for (int i = 0; i < myGameObjects.Count; i++)
            {
                if (myGameObjects[i].myRemove)
                {
                    myGameObjects.RemoveAt(i);
                    i--;
                }
            }

            #region Customise

            if (myPowerUpCount >= 5)
            {
                CustomizeState.myFirst = true;
            }
            if (myScore >= 50)
            {
                CustomizeState.mySecond = true;
            }
            if (myWallsDestroyed >= 10)
            {
                CustomizeState.myThird = true;
            }

            #endregion

            return true;
        }
        
        public void Reset()
        {
            myScore = 0;
            myPowerUpSpawnTime = 10;
            myCharachterPuSpawnTime = 30;
            myDisplayTextTimer = 2;
            myShowText = false;
            myBossTimer = 10;
            myPowerUpCoolDownSeconds = 10;
            mySuperPowerCoolDownSeconds = 8;
            myPowerUpCoolDown = false;
            myPowerUp = "";
            mySpawnWallsTimer = 5;
            mySpawnWalls = false;
            myRound = "1";
            myPowerUpCount = 0;
            myWallsDestroyed = 0;
            mySuperPowerUpUnlocked = false;
        }
        
        public void EnemySpawn(GameTime aGameTime)
        {
            if (aGameTime.TotalGameTime - myPreviousSpawnTime > myEnemySpawnTime)
            {
                int tempType = 0;

                if (myScore < 50)
                {
                    tempType = myRng.Next(1,5);
                }
                else if(myScore >= 50 && myScore < 100)
                {
                    tempType = myRng.Next(1, 4);
                }
                else if (myScore >= 100)
                {
                    tempType = myRng.Next(1, 5);
                }

                if (tempType == 1)
                {
                    myGameObjects.Add(new EnemyEasy(myEnemyTexture, new Vector2(myRng.Next(0, myGraphics.PreferredBackBufferWidth - myEnemyTexture.Width), myGraphics.PreferredBackBufferHeight + 20)));
                }
                if (tempType == 2)
                {
                    myGameObjects.Add(new EnemyMoving(myEnemyTexture, new Vector2(myRng.Next(0, myGraphics.PreferredBackBufferWidth - myEnemyTexture.Width), myGraphics.PreferredBackBufferHeight + 20)));
                }
                if (tempType == 3)
                {
                    Vector2 tempPos = new Vector2(myRng.Next(0, myGraphics.PreferredBackBufferWidth - myEnemyTexture.Width), myGraphics.PreferredBackBufferHeight + 20);
                    myGameObjects.Add(new EnemySmart(myEnemyTexture, tempPos));
                }
                if (tempType == 4)
                {
                    myGameObjects.Add(new ChargeEnemy(myEnemyTexture, new Vector2(myRng.Next(0, myGraphics.PreferredBackBufferWidth - myEnemyTexture.Width), myGraphics.PreferredBackBufferHeight + 20)));
                }

                myPreviousSpawnTime = aGameTime.TotalGameTime;
                float tempSpawnSeconds = 0;
                Rounds(tempSpawnSeconds);
            }
            mySpawnWallsTimer -= myDeltaTime;
            if (mySpawnWalls && mySpawnWallsTimer <= 0)
            {
                SpawnWalls();
                WallsRounds();
            }
        }

        
        public void Rounds(float aSeconds)
        {
            if (myScore < 10)
            {
                aSeconds = myRng.Next(3, 6);

                myRound = "1";
            }
            else if (myScore >= 10 && myScore < 20)
            {
                aSeconds = myRng.Next(3, 5);

                myRound = "2";
            }
            else if (myScore >= 20 && myScore < 50)
            {
                aSeconds = myRng.Next(2, 4);

                mySpawnWalls = true;
                myRound = "3";
            }
            else if (myScore >= 50 && myScore < 80)
            {
                aSeconds = myRng.Next(1, 3);
                myRound = "4";
            }
            else if (myScore >= 80 && myScore < 100)
            {
                aSeconds = 1;

                myRound = "5";
            }
            else if (myScore >= 100 && myScore < 150)
            {
                aSeconds = 0.5f;
                myRound = "6";
                for (int i = 0; i < myGameObjects.Count; i++)
                {
                    if (myGameObjects[i] is EnemyEasy || myGameObjects[i] is EnemyMoving)
                    {
                        myGameObjects[i].mySpeed += (1 * 0.1f);
                        (myGameObjects[i] as EnemyBase).myStartAttackTimer = 0.3f;
                    }
                }
            }
            else if (myScore >= 150)
            {
                aSeconds = 0.3f;
                myRound = "Endless";
                for (int i = 0; i < myGameObjects.Count; i++)
                {
                    if (myGameObjects[i] is EnemyEasy || myGameObjects[i] is EnemyMoving)
                    {
                        myGameObjects[i].mySpeed += (1 * 0.1f);
                        (myGameObjects[i] as EnemyBase).myStartAttackTimer = 0.2f;
                    }
                }
            }
            myEnemySpawnTime = TimeSpan.FromSeconds(aSeconds);
        }

        
        public void WallsRounds()
        {
            if (myScore >= 20 && myScore < 50)
            {
                mySpawnWallsTimer = myRng.Next(2, 5);
            }
            else if (myScore >= 50 && myScore < 100)
            {
                mySpawnWallsTimer = myRng.Next(2, 4);
            }
            else if (myScore >= 100)
            {
                mySpawnWallsTimer = 2;
            }
        }

        
        public void SpawnBoss()
        {
            myBossTimer -= myDeltaTime;
            int tempStyle = myRng.Next(1, 3);
            bool tempSpawnBoss = true;

            if (myBossTimer <= 0)
            {
                for (int i = 0; i < myGameObjects.Count; i++)
                {
                    if (myGameObjects[i] is EnemyBoss)
                    {
                        tempSpawnBoss = false;
                    }
                }
                if (tempSpawnBoss)
                {
                    myGameObjects.Add(new EnemyBoss(myEnemyTexture, tempStyle));
                }
            }
        }

        
        public void OutOfBounds()
        {
            for (int i = 0; i < myGameObjects.Count; i++)
            {
                if (myGameObjects[i].myPosition.Y >= myGraphics.PreferredBackBufferHeight + myGameObjects[i].myTexture.Height + 50 || myGameObjects[i].myPosition.Y <= 0 - myGameObjects[i].myTexture.Height || myGameObjects[i].myPosition.X >= myGraphics.PreferredBackBufferWidth + myGameObjects[i].myTexture.Width || myGameObjects[i].myPosition.X <= 0 - myGameObjects[i].myTexture.Width)
                {
                    myGameObjects[i].myRemove = true;
                }
            }
        }

        #region PowerUp
        
        public void PowerUpSpawn()
        {
            if (myPowerUpSpawnTime > 0)
            {
                myPowerUpSpawnTime -= myDeltaTime;
            }
            if (myPowerUpSpawnTime <= 0)
            {
                myGameObjects.Add(new SmallPowerUps(2f, myPowerupsTexture, new Vector2(myRng.Next(3, myGraphics.PreferredBackBufferWidth - myPowerupsTexture.Width), myGraphics.PreferredBackBufferHeight + 20), myRng.Next(1, 4), myPlayer, myGame));
                myPowerUpSpawnTime = myRng.Next(15, 20);
            }
        }
        
        public void SuperUpSpawn()
        {
            if (myCharachterPuSpawnTime > 0)
            {
                myCharachterPuSpawnTime -= myDeltaTime;
            }
            if (myCharachterPuSpawnTime <= 0)
            {
                myGameObjects.Add(new WeaponPowerUp(2f, myCharachterPuTexture, new Vector2(myRng.Next(3, myGraphics.PreferredBackBufferWidth - myPowerupsTexture.Width), myGraphics.PreferredBackBufferHeight + 20), myRng.Next(1, 101), myPlayer, myGame));
                myCharachterPuSpawnTime = myRng.Next(35, 50);
            }
        }
        
        public void PowerUpTimer(int aType, float aNormalFireSpeed, float aNormalSpeed)
        {
            myPowerUpCoolDownSeconds -= myDeltaTime;

            if (myPowerUpCoolDownSeconds <= 0)
            {
                if (aType == 2)
                {
                    myPlayer.mySpeed = aNormalSpeed;
                }
                if (aType == 1)
                {
                    myPlayer.myAttackSpeed = aNormalFireSpeed;
                }
                myPowerUpCoolDown = false;
            }
        }
        
        public void SuperPowerUpTimer(float aNormalFireSpeed)
        {
            mySuperPowerCoolDownSeconds -= myDeltaTime;

            if (mySuperPowerCoolDownSeconds <= 0)
            {
                myPlayer.myAttackSpeed = aNormalFireSpeed;
                myUltimateCoolDown = false;
            }
        }

        #endregion
        
        public void SpawnWalls()
        {
            int tempSpawnWay = myRng.Next(1, 3);

            if (tempSpawnWay == 1)
            {
                myGameObjects.Add(new Wall(myWallTexture, new Vector2(0, myGraphics.PreferredBackBufferHeight + 20)));
                myGameObjects.Add(new Wall(myWallTexture, new Vector2(myWallTexture.Width, myGraphics.PreferredBackBufferHeight + 20)));
                myGameObjects.Add(new Wall(myWallTexture, new Vector2(myWallTexture.Width * 2, myGraphics.PreferredBackBufferHeight + 20)));
            }
            else if (tempSpawnWay == 2)
            {
                myGameObjects.Add(new Wall(myWallTexture, new Vector2(myGraphics.PreferredBackBufferWidth - myWallTexture.Width, myGraphics.PreferredBackBufferHeight + 20)));
                myGameObjects.Add(new Wall(myWallTexture, new Vector2(myGraphics.PreferredBackBufferWidth - (myWallTexture.Width * 2), myGraphics.PreferredBackBufferHeight + 20)));
                myGameObjects.Add(new Wall(myWallTexture, new Vector2(myGraphics.PreferredBackBufferWidth - (myWallTexture.Width * 3), myGraphics.PreferredBackBufferHeight + 20)));
            }
        }
    }
}
