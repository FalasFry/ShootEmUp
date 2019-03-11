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
    public class GameState : States
    {
        Texture2D myPowerupsTexture;
        Texture2D myEnemyTexture;
        Texture2D myPlayerTexture;
        Texture2D myBullet;

        public List<Bullet> myBullets;
        public List<EnemyBasic> myEnemies;
        public List<PowerUp> myPowerUps;

        Player myPlayer;
        Random myRng;
        SpriteFont myFont;
        ParticleGenerator myStars;

        TimeSpan myPreviousSpawnTime;
        TimeSpan myEnemySpawnTime;
        GraphicsDeviceManager myGraphics;
        Texture2D myEnemyBullet;

        public float myScore;
        float myHealth = 10;
        float myDeltaTime;
        float myEnemyAttackTimer = 0;
        float myEnemyStartAttackTimer = 0.5f;
        float myTimer = 2;
        float myPowerUpCoolDownSeconds = 10;
        bool myPowerUpCoolDown;
        int myPowerUpType = 0;
        int myPowerUpIndex = 0;
        string myPowerUp;
        float myDisplayTextTimer = 2;
        bool myShowText;


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
            myEnemies = new List<EnemyBasic>();
            myPowerUps = new List<PowerUp>();

            myPlayer = new Player(myGame, myPlayerTexture)
            {
                myHp = myHealth,
            };
        }

        public override void Draw(GameTime aGameTime, SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Begin();

            myStars.Draw(aSpriteBatch);
            
            for (int i = 0; i < myBullets.Count; i++)
            {
                myBullets[i].DrawBullet(aSpriteBatch);
            }
            for (int i = 0; i < myEnemies.Count; i++)
            {
                myEnemies[i].Draw(aSpriteBatch);
            }
            for (int i = 0; i < myPowerUps.Count; i++)
            {
                myPowerUps[i].Draw(aSpriteBatch);
            }
            
            aSpriteBatch.DrawString(myFont, "HP Left: " +myPlayer.myHp.ToString(), new Vector2(400, 0), Color.White);
            aSpriteBatch.DrawString(myFont, "Score: " + myScore, new Vector2(200, 0), Color.White);

            myPlayer.Draw(aSpriteBatch);

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
            myPlayer.Update(aGameTime);
            OutOfBounds();
            Collision();
            EnemySpawn(aGameTime);
            PowerUpSpawn();
            for (int i = 0; i < myBullets.Count; i++)
            {
                myBullets[i].Update();
            }
            for (int i = 0; i < myEnemies.Count; i++)
            {
                myEnemies[i].Update(aGameTime);
            }
            for (int i = 0; i < myPowerUps.Count; i++)
            {
                myPowerUps[i].Update(aGameTime);
            }
            #endregion

            if(myShowText)
            {
                myDisplayTextTimer -= myDeltaTime;

                if(myDisplayTextTimer <= 0)
                {
                    myShowText = false;
                    myDisplayTextTimer = 2;
                }
            }

            #region Shooting
            if (tempMouse.LeftButton == ButtonState.Pressed || tempKeyboard.IsKeyDown(Keys.J))
            {
                if (myPlayer.myAttackTimer <= 0)
                {
                    Shoot();
                    myPlayer.myAttackTimer = myPlayer.myAttackSpeed;
                }
            }
            myPlayer.myAttackTimer -= myDeltaTime;

            if(myPlayer.myHp <= 0)
            {
                myPlayer.myDead = true;
                myGame.ChangeState(new GameOverState(myGame, myGraphDevice, myContentManager, myScore, myGraphics));
            }

            myEnemyAttackTimer -= myDeltaTime;
            if(myEnemyAttackTimer <= 0)
            {
                EnemyShoot();
            }
            #endregion

            return true;
        }

        public void Collision()
        {
            for (int i = 0; i < myBullets.Count; i++)
            {
                for (int j = 0; j < myEnemies.Count; j++)
                {
                    if (myBullets[i].myOwner == 1)
                    {
                        if (myBullets[i].myRectangle.Intersects(myEnemies[j].myRectangle))
                        {
                            DestroyEnemy(j);
                            DestroyBullet(i);
                            myScore++;
                        }
                    }
                }
            }

            for (int i = 0; i < myBullets.Count; i++)
            {
                if (myBullets[i].myOwner == 2)
                {
                    if (myBullets[i].myRectangle.Intersects(myPlayer.myRectangle))
                    {
                        DestroyBullet(i);
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
                    DestroyPowerUp(i);
                }
            }
            if (myPowerUpCoolDown)
            {
                PowerUpTimer(myPowerUpType, myPowerUpIndex);
            }
        }

        public void Shoot()
        {
            myBullets.Add(new Bullet(7, new Vector2(0,1), myBullet, (myPlayer.myPosition+myPlayer.myBulletsSpawn), 1, Color.White));
        }

        public void EnemyShoot()
        {
            for (int i = 0; i < myEnemies.Count; i++)
            {
                myBullets.Add(new Bullet(7, new Vector2(0, -1), myEnemyBullet, myEnemies[i].myPosition + myEnemies[i].myBulletSpawn, 2, Color.Cyan));
            }
            myEnemyAttackTimer = myEnemyStartAttackTimer;
            
        }

        public void EnemySpawn(GameTime aGameTime)
        {
            if (aGameTime.TotalGameTime - myPreviousSpawnTime > myEnemySpawnTime)
            {
                int tempType = myRng.Next(1, 3);
                if(tempType == 1)
                {
                    myEnemies.Add(new EnemyEasy(myEnemyTexture, new Vector2(myRng.Next(myEnemyTexture.Width, myGraphics.PreferredBackBufferWidth - myEnemyTexture.Width), myGraphics.PreferredBackBufferHeight + 20)));
                }
                if(tempType == 2)
                {
                    myEnemies.Add(new EnemyMoving(myEnemyTexture, new Vector2(myRng.Next(myEnemyTexture.Width, myGraphics.PreferredBackBufferWidth - myEnemyTexture.Width), myGraphics.PreferredBackBufferHeight + 20)));
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

        public void DestroyEnemy(int index)
        {
            myEnemies.RemoveAt(index);
        }

        public void DestroyBullet(int index)
        {
            myBullets.RemoveAt(index);
        }

        public void DestroyPowerUp(int index)
        {
            myPowerUps.RemoveAt(index);
        }

        public void OutOfBounds()
        {
            for (int i = 0; i < myEnemies.Count; i++)
            {
                if(myEnemies[i].myPosition.Y <= 0-myEnemyTexture.Height)
                {
                    DestroyEnemy(i);
                }
            }

            for (int i = 0; i < myBullets.Count; i++)
            {
                if (myBullets[i].myPosition.Y >= myGraphics.PreferredBackBufferHeight + myBullet.Height)
                {
                    DestroyBullet(i);
                }
            }
        }

        public void PowerUpSpawn()
        {
            // Timer start as 0, thats why you always get a powerup at start.
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
