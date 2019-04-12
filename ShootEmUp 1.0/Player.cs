using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ShootEmUp_1._0
{
    class Player : GameObject
    {
        public float myHp = 100;
        public float myAttackSpeed = 0.5f;
        public float myAttackTimer = 0;

        public Vector2 myPrevPos;
        public Vector2 myBulletsSpawn;
        public bool myNormalFire = true;
        public bool myUltimate = false;
        public int myFireType;
        public float myBaseAttackSpeed;


        public Player(Texture2D aTexture)
        {
            myRotation = 0;
            myTexture = aTexture;
            mySpeed = 7;
            myPosition = new Vector2(300, myTexture.Height);
            myRectangle = new Rectangle(0, 0, myTexture.Width * (int)myScale, myTexture.Height * (int)myScale);
            myBulletsSpawn = new Vector2(myRectangle.Width * 0.5f, myRectangle.Height);
            myBaseAttackSpeed = myAttackSpeed;
        }

        public override void Update(GameTime aGameTime)
        {
            Movement();
            FireCheck();
            myRectangle.Location = myPosition.ToPoint();
        }

        public void FireCheck()
        {
            MouseState tempMouse = Mouse.GetState();
            KeyboardState tempKeyboard = Keyboard.GetState();

            if (tempMouse.LeftButton == ButtonState.Pressed || tempKeyboard.IsKeyDown(Keys.J) || tempKeyboard.IsKeyDown(Keys.K) || tempKeyboard.IsKeyDown(Keys.L))
            {
                int tempDirX = 0;
                if (tempKeyboard.IsKeyDown(Keys.K))
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

                if (myAttackTimer <= 0)
                {
                    if (myNormalFire)
                    {
                        Shoot(tempDirX);
                    }
                    else if (!myNormalFire)
                    {
                        ShootAllAtOnce(myFireType);
                    }
                    myAttackTimer = myAttackSpeed;
                }
            }
            myAttackTimer -= GameState.myDeltaTime;
        }

        public void Movement()
        {
            KeyboardState tempKeyState = Keyboard.GetState();
            Vector2 tempDir = new Vector2();

            if (tempKeyState.IsKeyDown(Keys.A))
            {
                if(myPosition.X >= 0 )
                {
                    tempDir.X = -1;
                }
            }
            if (tempKeyState.IsKeyDown(Keys.D))
            {
                if(myPosition.X <= (700 - myTexture.Width))
                {
                    tempDir.X = 1;
                }
            }
            if (tempKeyState.IsKeyDown(Keys.W))
            { 
                if(myPosition.Y >= 0)
                {
                    tempDir.Y = -1;
                }
            }
            if (tempKeyState.IsKeyDown(Keys.S))
            {
                if (myPosition.Y <= ((900/1.5f) - myTexture.Height))
                {
                    tempDir.Y = 1;
                }
            }
            if (tempDir.X > 1f || tempDir.Y > 1f)
            {
                tempDir.Normalize();
            }
            myPrevPos = tempDir;
            if (tempDir == Vector2.Zero)
            {
                tempDir = myPrevPos;
            }

            if (tempKeyState.IsKeyDown(Keys.S) || tempKeyState.IsKeyDown(Keys.W) || tempKeyState.IsKeyDown(Keys.D) || tempKeyState.IsKeyDown(Keys.A))
            {
                myPosition += (tempDir * mySpeed);
            }
        }

        public void Shoot(int aDirX)
        {
            GameState.myGameObjects.Add(new Bullet(7, new Vector2(aDirX, 1), GameState.myBullet, (myPosition + myBulletsSpawn), 1, Color.White));
        }

        public void ShootAllAtOnce(int aRandom)
        {
            if (aRandom == 1)
            {
                GameState.myGameObjects.Add(new Bullet(7, new Vector2(1, 1), GameState.myBullet, (myPosition + myBulletsSpawn), 1, Color.White));
                GameState.myGameObjects.Add(new Bullet(7, new Vector2(-1, 1), GameState.myBullet, (myPosition + myBulletsSpawn), 1, Color.White));
                GameState.myGameObjects.Add(new Bullet(7, new Vector2(0, 1), GameState.myBullet, (myPosition + myBulletsSpawn), 1, Color.White));
            }
            else if(aRandom == 2)
            {
                GameState.myGameObjects.Add(new Bullet(7, new Vector2(0, 1), GameState.myBullet, (myPosition + myBulletsSpawn + new Vector2(-50, 0)) , 1, Color.White));
                GameState.myGameObjects.Add(new Bullet(7, new Vector2(0, 1), GameState.myBullet, (myPosition + myBulletsSpawn + new Vector2(50, 0)), 1, Color.White));
                GameState.myGameObjects.Add(new Bullet(7, new Vector2(0, 1), GameState.myBullet, (myPosition + myBulletsSpawn), 1, Color.White));
            }
        }
    }
}
