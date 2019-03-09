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
        public List<Enemy> myEnemies;

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
        

        public GameState(Game1 aGame, GraphicsDevice aGraphicsDevice, ContentManager aContent, GraphicsDeviceManager aManager) : base(aGame, aGraphicsDevice, aContent)
        {
            myGraphics = aManager;
            aManager.PreferredBackBufferHeight = 900;
            aManager.PreferredBackBufferWidth = 700;
            aManager.ApplyChanges();
            myEnemyTexture = aContent.Load<Texture2D>("EnemyShip");
            myPowerupsTexture = aContent.Load<Texture2D>("ball");
            myPlayerTexture = aContent.Load<Texture2D>("PlayerShip");
            myBullet = aContent.Load<Texture2D>("BulletPixel");
            myEnemyBullet = aContent.Load<Texture2D>("ball");
            myFont = aContent.Load<SpriteFont>("Font");
            myRng = new Random();
            myStars = new ParticleGenerator(aContent.Load<Texture2D>("Star"), aManager.PreferredBackBufferWidth, 100);

            myBullets = new List<Bullet>();
            myEnemies = new List<Enemy>();

            myPlayer = new Player(myGame, myPlayerTexture)
            {
                myHp = myHealth,
            };

            for (int i = 0; i < 50; i++)
            {
                
            }
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
            
            aSpriteBatch.DrawString(myFont, "HP Left: " +myHealth.ToString(), new Vector2(400, 0), Color.White);
            aSpriteBatch.DrawString(myFont, "Score: " + myScore, new Vector2(200, 0), Color.White);

            myPlayer.Draw(aSpriteBatch);

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
            for (int i = 0; i < myBullets.Count; i++)
            {
                myBullets[i].Update();
            }
            for (int i = 0; i < myEnemies.Count; i++)
            {
                myEnemies[i].Update(aGameTime);
            }
            #endregion

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

            if(myHealth <= 0)
            {
                myPlayer.myDead = true;
                myGame.ChangeState(new GameOverState(myGame, myGraphDevice, myContentManager));
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
                        myHealth--;
                    }
                }
            }

            for (int i = 0; i < myEnemies.Count; i++)
            {
                if (myEnemies[i].myRectangle.Intersects(myPlayer.myRectangle))
                {
                    DestroyEnemy(i);
                    myHealth--;
                }
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
                myEnemies.Add(new Enemy(myEnemyTexture, 1, new Vector2((myRng.Next(myEnemyTexture.Width, myGraphics.PreferredBackBufferWidth-myEnemyTexture.Width)), myGraphics.PreferredBackBufferHeight + 20), myRng.Next(1, 3)));

                myPreviousSpawnTime = aGameTime.TotalGameTime;
                float tempSpawnSeconds = 0;

                if (aGameTime.ElapsedGameTime.TotalSeconds < 60)
                {
                    tempSpawnSeconds = myRng.Next(1, 4);
                }
                else if(aGameTime.ElapsedGameTime.TotalSeconds > 60 && aGameTime.ElapsedGameTime.TotalSeconds < 120)
                {
                    tempSpawnSeconds = myRng.Next(1, 3);
                }
                else if (aGameTime.ElapsedGameTime.TotalSeconds > 120)
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
    }
}
