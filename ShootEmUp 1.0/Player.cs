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
        public float myHp = 20;
        public float myAttackSpeed = 0.5f;
        public float myAttackTimer = 0;

        public Vector2 myPrevPos;
        public Vector2 myBulletsSpawn;
        float myUltimateCooldown;


        public Player(Texture2D aTexture)
        {
            myUltimateCooldown = 10;
            myRotation = 0;
            myTexture = aTexture;
            mySpeed = 7;
            myPosition = new Vector2(300, myTexture.Height);
            myRectangle = new Rectangle(0, 0, myTexture.Width * (int)myScale, myTexture.Height * (int)myScale);
            myBulletsSpawn = new Vector2(myRectangle.Width * 0.5f, myRectangle.Height);
        }

        public override void Update(GameTime aGameTime)
        {
            MouseState tempMouse = Mouse.GetState();
            KeyboardState tempKeyboard = Keyboard.GetState();

            Movement();
            myRectangle.Location = myPosition.ToPoint();

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
                    Shoot(tempDirX);
                    myAttackTimer = myAttackSpeed;
                }
            }
            myAttackTimer -= GameState.myDeltaTime;

            myUltimateCooldown -= GameState.myDeltaTime;
            if (tempKeyboard.IsKeyDown(Keys.R) && myUltimateCooldown <= 0)
            {
                Ultimate();
                myUltimateCooldown = 10;
            }
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

        public void Ultimate()
        {
            myScale = 5;
            myHp += 10;
        }
    }
}
