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
        float myMoveTimer = 15;
        float mySpeed = 1;
        public float myScore;
        float myHealth = 10;

        Player myPlayer;
        Texture2D myBullet;
        Random myRng = new Random();
        SpriteFont myFont;
        Texture2D myPowerupsTexture;
        Texture2D myEnemyTexture;
        Texture2D myPlayerTexture;

        float myDeltaTime;

        List<Bullet> myBullets;
        List<Enemy> myEnemies;


        public GameState(Game1 aGame, GraphicsDevice aGraphicsDevice, ContentManager aContent, GraphicsDeviceManager aManager) : base(aGame, aGraphicsDevice, aContent)
        {
            aManager.PreferredBackBufferHeight = 900;
            aManager.PreferredBackBufferWidth = 700;
            aManager.ApplyChanges();
            myEnemyTexture = aContent.Load<Texture2D>("EnemyShip");
            myPowerupsTexture = aContent.Load<Texture2D>("ball");
            myPlayerTexture = aContent.Load<Texture2D>("PlayerShip");
            myBullet = aContent.Load<Texture2D>("BulletPixel");

            myBullets = new List<Bullet>()
            {
                new Bullet(0, new Vector2(1,0), myBullet, new Vector2(1000,1000), 1, Color.White),
            };
            myEnemies = new List<Enemy>()
            {
                new Enemy(myEnemyTexture, 1f, new Vector2(1000,1000), 1),
            };
            myPlayer = new Player(myGame, myPlayerTexture)
            {
                myHp = myHealth,
            };
        }

        public override void Draw(GameTime aGameTime, SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Begin();

            for (int i = 0; i < myBullets.Count; i++)
            {
                myBullets[i].DrawBullet(aSpriteBatch);
            }
            for (int i = 0; i < myEnemies.Count; i++)
            {
                myEnemies[i].Draw(aSpriteBatch);
            }


            myPlayer.Draw(aSpriteBatch);

            aSpriteBatch.End();
        }

        public override bool Update(GameTime aGameTime)
        {
            myDeltaTime = (float)aGameTime.ElapsedGameTime.TotalSeconds;
            MouseState tempMouse = Mouse.GetState();
            KeyboardState tempKeyboard = Keyboard.GetState();
            myPlayer.Update(aGameTime);


            for (int i = 0; i < myBullets.Count; i++)
            {
                myBullets[i].Update();
            }

            if(tempMouse.LeftButton == ButtonState.Pressed || tempKeyboard.IsKeyDown(Keys.J))
            {
                if (myPlayer.myAttackTimer <= 0)
                {
                    Shoot();
                    myPlayer.myAttackTimer = myPlayer.myAttackSpeed;
                }
            }
            myPlayer.myAttackTimer -= myDeltaTime;
            return true;
        }

        public void Shoot()
        {
            myBullets.Add(new Bullet(5, new Vector2(0,1), myBullet, (myPlayer.myPosition+myPlayer.myBulletsSpawn), 1, Color.White));
        }
    }
}
