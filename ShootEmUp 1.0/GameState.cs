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
        Texture2D myWallTexture;

        public static Texture2D myBullet;
        public static Texture2D myEnemyBullet;
        public static List<GameObject> myGameObjects;

        Player myPlayer;
        Random myRng;
        SpriteFont myFont;
        ParticleGenerator myStars;

        TimeSpan myPreviousSpawnTime;
        TimeSpan myEnemySpawnTime;
        GraphicsDeviceManager myGraphics;

        public static float myScore;
        public static float myDeltaTime;
        float myPowerUpSpawnTime = 2;
        public static float myDisplayTextTimer = 2;
        public static bool myShowText;
        public static float myBossTimer = 5;
        public static float myPowerUpCoolDownSeconds = 10;
        public static bool myPowerUpCoolDown;
        public static string myPowerUp;

        public GameState(Game1 aGame, GraphicsDevice aGraphicsDevice, ContentManager aContent, GraphicsDeviceManager aManager) : base(aGame, aGraphicsDevice, aContent)
        {
            myGraphics = aManager;
            aManager.PreferredBackBufferHeight = 900;
            aManager.PreferredBackBufferWidth = 700;
            aManager.ApplyChanges();
            myScore = 0;

            myEnemyTexture = aContent.Load<Texture2D>("EnemyShip");
            myPowerupsTexture = aContent.Load<Texture2D>("PowerUp");
            myPlayerTexture = aContent.Load<Texture2D>("PlayerShip");
            myBullet = aContent.Load<Texture2D>("BulletPixel");
            myEnemyBullet = aContent.Load<Texture2D>("ball");
            myFont = aContent.Load<SpriteFont>("Font");
            myWallTexture = aContent.Load<Texture2D>("Walls");
            myRng = new Random();
            myStars = new ParticleGenerator(aContent.Load<Texture2D>("Star"), aManager.PreferredBackBufferWidth, 100);

            myGameObjects = new List<GameObject>();
            myPlayer = new Player(myPlayerTexture);

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

            #region Updating
            myStars.Update(aGameTime, myGraphDevice);
            OutOfBounds();
            SpawnBoss();
            EnemySpawn(aGameTime);
            PowerUpSpawn();

            for (int i = 0; i < myGameObjects.Count; i++)
            {
                myGameObjects[i].Update(aGameTime);
            }

            if (GameState.myPowerUpCoolDown)
            {
                PowerUpTimer(PowerUp.myPowerType, PowerUp.myPowerUpIndex);
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

            if(myScore <= 0)
            {
                myScore = 0;
            }

            if(myPlayer.myHp <= 0)
            {
                myGame.ChangeState(new GameOverState(myGame, myGraphDevice, myContentManager, myScore, myGraphics));
            }

            for (int i = 0; i < myGameObjects.Count; i++)
            {
                if(myGameObjects[i].myRemove)
                {
                    myGameObjects.RemoveAt(i);
                    i--;
                }
            }
                  
            return true;
        }

        public void EnemySpawn(GameTime aGameTime)
        {
            if (aGameTime.TotalGameTime - myPreviousSpawnTime > myEnemySpawnTime)
            {
                //bool tempSpawnWalls = false;
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
                    //tempSpawnWalls = true;
                }

                //if(tempSpawnWalls)
                //{
                //    SpawnWalls();
                //}
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

        public void OutOfBounds()
        {
            for (int i = 0; i < myGameObjects.Count; i++)
            {
                if (myGameObjects[i].myPosition.Y >= myGraphics.PreferredBackBufferHeight + myBullet.Height || myGameObjects[i].myPosition.Y <= 0 - myGameObjects[i].myTexture.Height || myGameObjects[i].myPosition.X >= myGraphics.PreferredBackBufferWidth + myGameObjects[i].myTexture.Width || myGameObjects[i].myPosition.X <= 0 - myGameObjects[i].myTexture.Width) 
                {
                    myGameObjects[i].myRemove = true;
                }
            }
        }

        public void PowerUpSpawn()
        {
            if (myPowerUpSpawnTime > 0)
            {
                myPowerUpSpawnTime -= myDeltaTime;
            }
            if (myPowerUpSpawnTime <= 0)
            {
                myGameObjects.Add(new PowerUp(2f, myPowerupsTexture, new Vector2(myRng.Next(3,myGraphics.PreferredBackBufferWidth-myPowerupsTexture.Width), myGraphics.PreferredBackBufferHeight+20),myRng.Next(1,4) ,myPlayer, myGame));
                myPowerUpSpawnTime = myRng.Next(15, 30);
            }
        }

        public void SpawnWalls()
        {
            int tempSpawnWay = myRng.Next(1,3);

            if(tempSpawnWay == 1)
            {
                myGameObjects.Add(new Wall(myWallTexture, new Vector2(0, myGraphics.PreferredBackBufferHeight + 20)));
                myGameObjects.Add(new Wall(myWallTexture, new Vector2(myWallTexture.Width, myGraphics.PreferredBackBufferHeight + 20)));
                myGameObjects.Add(new Wall(myWallTexture, new Vector2(myWallTexture.Width * 2, myGraphics.PreferredBackBufferHeight + 20)));
            }
            else if(tempSpawnWay == 2)
            {
                myGameObjects.Add(new Wall(myWallTexture, new Vector2(myGraphics.PreferredBackBufferWidth - myWallTexture.Width, myGraphics.PreferredBackBufferHeight + 20)));
                myGameObjects.Add(new Wall(myWallTexture, new Vector2(myGraphics.PreferredBackBufferWidth - (myWallTexture.Width * 2), myGraphics.PreferredBackBufferHeight + 20)));
                myGameObjects.Add(new Wall(myWallTexture, new Vector2(myGraphics.PreferredBackBufferWidth - (myWallTexture.Width * 3), myGraphics.PreferredBackBufferHeight + 20)));
            }
        }

        public void PowerUpTimer(int aType, int index)
        {
            GameState.myPowerUpCoolDownSeconds -= myDeltaTime;

            if (GameState.myPowerUpCoolDownSeconds <= 0)
            {
                if (aType == 2)
                {
                    myPlayer.mySpeed = 7;
                }
                if (aType == 1)
                {
                    myPlayer.myAttackSpeed = 0.5f;
                }
                GameState.myPowerUpCoolDown = false;
            }
        }
    }
}
