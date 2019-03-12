using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace ShootEmUp_1._0
{
    class GameState : States
    {
        Texture2D myPowerupsTexture;
        Texture2D myEnemyTexture;
        Texture2D myPlayerTexture;
        Texture2D myBullet;
        public static Texture2D myEnemyBullet;

        public static List<GameObject> myGameObjects;
        public List<Bullet> myBullets;
        public List<EnemyBase> myEnemies;
        public List<PowerUp> myPowerUps;
        public List<EnemyBase> myBosses;

        Player myPlayer;
        Random myRng;
        SpriteFont myFont;
        ParticleGenerator myStars;

        TimeSpan myPreviousSpawnTime;
        TimeSpan myEnemySpawnTime;
        GraphicsDeviceManager myGraphics;

        public float myScore;
        float myHealth = 10000;
        float myDeltaTime;
        float myEnemyAttackTimer = 0;
        float myBossAS = 0.3f;
        float myTimer = 2;
        float myPowerUpCoolDownSeconds = 10;
        bool myPowerUpCoolDown;
        int myPowerUpType = 0;
        int myPowerUpIndex = 0;
        string myPowerUp;
        float myDisplayTextTimer = 2;
        bool myShowText;
        float myBossTimer = 5;



        public GameState(Game1 aGame, GraphicsDevice aGraphicsDevice, ContentManager aContent, GraphicsDeviceManager aManager) : base(aGame, aGraphicsDevice, aContent)
        {
            myGraphics = aManager;
            aManager.PreferredBackBufferHeight = 900;
            aManager.PreferredBackBufferWidth = 700;
            aManager.ApplyChanges();
            myEnemyTexture = aContent.Load<Texture2D>("EnemyShip");
            myPowerupsTexture = aContent.Load<Texture2D>("PowerUp");
            myPlayerTexture = aContent.Load<Texture2D>("PlayerShip");
            myBullet = aContent.Load<Texture2D>("BulletPixel");
            myEnemyBullet = aContent.Load<Texture2D>("ball");
            myFont = aContent.Load<SpriteFont>("Font");
            myRng = new Random();
            myStars = new ParticleGenerator(aContent.Load<Texture2D>("Star"), aManager.PreferredBackBufferWidth, 100);

            myBullets = new List<Bullet>();
            myEnemies = new List<EnemyBase>();
            myPowerUps = new List<PowerUp>();
            myBosses = new List<EnemyBase>();
            myGameObjects = new List<GameObject>();



            myPlayer = new Player(myPlayerTexture)
            {
                myHp = myHealth,
            };

            myGameObjects.Add(myPlayer);
        }

        public override void Draw(GameTime aGameTime, SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Begin();

            myStars.Draw(aSpriteBatch);

            for (int i = 0; i < myGameObjects.Count; i++)
            {
                myGameObjects[i].Draw(aSpriteBatch);
            }

            aSpriteBatch.DrawString(myFont, "HP Left: " +myPlayer.myHp.ToString(), new Vector2(400, 0), Color.White);
            aSpriteBatch.DrawString(myFont, "Score: " + myScore, new Vector2(200, 0), Color.White);


            int tempTextOffsetX = 100;
            int tempTextOffsetY = 50;
            if (myPowerUpCoolDown)
            {
                string tempText = "PowerUp Time Left " + (int)myPowerUpCoolDownSeconds;
                aSpriteBatch.DrawString(myFont, tempText, new Vector2((myGraphics.PreferredBackBufferWidth / 2 - tempTextOffsetX), myGraphics.PreferredBackBufferHeight / 2), Color.White);
            }
            if (myShowText)
            {
                string tempText = "You Got " + myPowerUp;
                aSpriteBatch.DrawString(myFont, tempText, new Vector2((myGraphics.PreferredBackBufferWidth/2 - tempTextOffsetX), myGraphics.PreferredBackBufferHeight / 2 - tempTextOffsetY), Color.White);
            }

            aSpriteBatch.End();
        }

        public override bool Update(GameTime aGameTime)
        {
            myDeltaTime = (float)aGameTime.ElapsedGameTime.TotalSeconds;
            MouseState tempMouse = Mouse.GetState();
            KeyboardState tempKeyboard = Keyboard.GetState();

            myStars.Update(aGameTime, myGraphDevice);

            #region Updating
            OutOfBounds();
            Collision();
            SpawnBoss();
            EnemySpawn(aGameTime);

            PowerUpSpawn();

            for (int i = 0; i < myGameObjects.Count; i++)
            {
                myGameObjects[i].Update(aGameTime);
            }
            #endregion

            if (myShowText)
            {
                myDisplayTextTimer -= myDeltaTime;

                if(myDisplayTextTimer <= 0)
                {
                    myShowText = false;
                    myDisplayTextTimer = 2;
                }
            }

            #region Shooting
            if (tempMouse.LeftButton == ButtonState.Pressed || tempKeyboard.IsKeyDown(Keys.J) || tempKeyboard.IsKeyDown(Keys.K) || tempKeyboard.IsKeyDown(Keys.L))
            {
                int tempDirX = 0;
                if(tempKeyboard.IsKeyDown(Keys.K))
                {
                    tempDirX = 0;
                }
                if (tempKeyboard.IsKeyDown(Keys.J))
                {
                    tempDirX = -1;
                }
                if (tempKeyboard.IsKeyDown(Keys.L))
                {
                    tempDirX = 1;
                }
                if (myPlayer.myAttackTimer <= 0)
                {
                    Shoot(tempDirX);
                    myPlayer.myAttackTimer = myPlayer.myAttackSpeed;
                }
            }
            myPlayer.myAttackTimer -= myDeltaTime;

            if(myPlayer.myHp <= 0)
            {
                myGame.ChangeState(new GameOverState(myGame, myGraphDevice, myContentManager, myScore, myGraphics));
            }

            myEnemyAttackTimer -= myDeltaTime;


            #endregion

            return true;
        }

        public void Collision()
        {
            for (int i = 0; i < myBullets.Count; i++)
            {
                for (int j = 0; j < myBosses.Count; j++)
                {
                    if (myBullets[i].myOwner == 1)
                    {
                        if (myBullets[i].myRectangle.Intersects(myBosses[j].myRectangle) && myBosses[j].myHealth > 0)
                        {
                            myBosses[j].myHealth--;
                            myGameObjects[i].myRemove = true;
                            i--;
                        }
                    }
                    if (myBosses[j].myHealth <= 0)
                    {
                        DestroyBoss(j);
                        j--;
                        myScore++;
                    }
                }
            }
            

            for (int i = 0; i < myBullets.Count; i++)
            {
                if (myBullets[i].myOwner == 2)
                {
                    if (myBullets[i].myRectangle.Intersects(myPlayer.myRectangle))
                    {
                        myGameObjects[i].myRemove = true;
                        myPlayer.myHp--;
                    }
                }
            }

            for (int i = 0; i < myEnemies.Count; i++)
            {
                if (myEnemies[i].myRectangle.Intersects(myPlayer.myRectangle))
                {
                    DestroyEnemy(i);
                    myPlayer.myHp--;
                }
            }

            for (int i = 0; i < myPowerUps.Count; i++)
            {
                if(myPowerUps[i].myRectangle.Intersects(myPlayer.myRectangle))
                {
                    if (myPowerUps[i].myPowerType == 1)
                    {
                        myPowerUp = "More AttackSpeed";
                        myPlayer.myAttackSpeed = 0.1f;
                        myPowerUpCoolDownSeconds = 5f;
                        myPowerUpCoolDown = true;
                    }
                    if (myPowerUps[i].myPowerType == 2)
                    {
                        myPowerUp = "More Speed";
                        myPlayer.mySpeed = 13;
                        myPowerUpCoolDownSeconds = 10f;
                        myPowerUpCoolDown = true;
                    }
                    if (myPowerUps[i].myPowerType == 3)
                    {
                        myPowerUp = "+5 HP";
                        myPlayer.myHp += 5;
                    }
                    myShowText = true;
                    myPowerUpType = myPowerUps[i].myPowerType;
                    myPowerUpIndex = i;
                }
            }
            if (myPowerUpCoolDown)
            {
                PowerUpTimer(myPowerUpType, myPowerUpIndex);
            }
        }

        public void Shoot(int aDirX)
        {
            myGameObjects.Add(new Bullet(7, new Vector2(aDirX,1), myBullet, (myPlayer.myPosition+myPlayer.myBulletsSpawn), 1, Color.White));
        }

        public void EnemySpawn(GameTime aGameTime)
        {
            if (aGameTime.TotalGameTime - myPreviousSpawnTime > myEnemySpawnTime)
            {
                int tempType = myRng.Next(1, 3);
                if(tempType == 1)
                {
                    myGameObjects.Add(new EnemyEasy(myEnemyTexture, new Vector2(myRng.Next(myEnemyTexture.Width, myGraphics.PreferredBackBufferWidth - myEnemyTexture.Width), myGraphics.PreferredBackBufferHeight + 20)));
                }
                if(tempType == 2)
                {
                    myGameObjects.Add(new EnemyMoving(myEnemyTexture, new Vector2(myRng.Next(myEnemyTexture.Width, myGraphics.PreferredBackBufferWidth - myEnemyTexture.Width), myGraphics.PreferredBackBufferHeight + 20)));
                }
 

                myPreviousSpawnTime = aGameTime.TotalGameTime;
                float tempSpawnSeconds = 0;

                if (aGameTime.ElapsedGameTime.TotalSeconds < 60)
                {
                    tempSpawnSeconds = myRng.Next(1, 3);
                }
                else if(aGameTime.ElapsedGameTime.TotalSeconds > 60)
                {
                    tempSpawnSeconds = 1;
                }
                myEnemySpawnTime = TimeSpan.FromSeconds(tempSpawnSeconds);
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
                if(tempSpawnBoss)
                {
                    myGameObjects.Add(new EnemyBoss(myEnemyTexture, tempStyle));
                }
            }

        }

        public void DestroyEnemy(int index)
        {
            myEnemies.RemoveAt(index);
        }

        public void DestroyBoss(int index)
        {
            float tempNr = 5;
            myBossTimer = tempNr;
            myBosses.RemoveAt(index);
        }

        public void DestroyGameObject(int index)
        {
            myGameObjects.RemoveAt(index);
        }

        public void OutOfBounds()
        {
            for (int i = 0; i < myGameObjects.Count; i++)
            {
                if (myGameObjects[i].myPosition.Y >= myGraphics.PreferredBackBufferHeight + myBullet.Height || myGameObjects[i].myPosition.Y <= 0 - myGameObjects[i].myTexture.Height || myGameObjects[i].myPosition.X >= myGraphics.PreferredBackBufferWidth + myGameObjects[i].myTexture.Width || myGameObjects[i].myPosition.X <= 0 + myGameObjects[i].myTexture.Width) 
                {
                    myGameObjects[i].myRemove = true;
                }
            }
        }

        public void PowerUpSpawn()
        {
            if (myTimer > 0)
            {
                myTimer -= myDeltaTime;
            }
            if (myTimer <= 0)
            {
                myPowerUps.Add(new PowerUp(2f, myPowerupsTexture, new Vector2(myRng.Next(3,myGraphics.PreferredBackBufferWidth-myPowerupsTexture.Width), myGraphics.PreferredBackBufferHeight+20),myRng.Next(1,4) ,myPlayer, myGame));
                myTimer = myRng.Next(15, 30);
            }
        }

        public void PowerUpTimer(int aType, int index)
        {
            myPowerUpCoolDownSeconds -= myDeltaTime;

            if (myPowerUpCoolDownSeconds <= 0)
            {
                if (aType == 2)
                {
                    myPlayer.mySpeed = 7;
                }
                if (aType == 1)
                {
                    myPlayer.myAttackSpeed = 0.5f;
                }
                myPowerUpCoolDown = false;
            }
        }
    }
}
