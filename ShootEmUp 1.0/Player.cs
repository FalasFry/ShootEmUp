﻿using Microsoft.Xna.Framework;
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
        public float myHp = 5;
        public float myAttackSpeed = 0.5f;
        public float myAttackTimer = 0;

        public Vector2 myPrevPos;
        public Vector2 myBulletsSpawn;


        public Player(Texture2D aTexture)
        {
            myTexture = aTexture;
            mySpeed = 7;
            myOffset = ((myTexture.Bounds.Size.ToVector2() * 0.5f));
            myBulletsSpawn = new Vector2(myTexture.Bounds.Size.X * 0.5f, myTexture.Bounds.Size.Y);
            myPosition = new Vector2(300, 10);
            myRectangle = new Rectangle((myOffset).ToPoint(), (aTexture.Bounds.Size.ToVector2()).ToPoint());
        }

        public override void Update(GameTime aGameTime)
        {
            Movement();
            myRectangle.Location = myPosition.ToPoint();
        }

        public void Movement()
        {
            KeyboardState keyState = Keyboard.GetState();
            Vector2 tempDir = new Vector2();

            if (keyState.IsKeyDown(Keys.A))
            {
                if(myPosition.X >= 0 )
                {
                    tempDir.X = -1;
                }
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                if(myPosition.X <= (700 - myTexture.Width))
                {
                    tempDir.X = 1;
                }

            }
            if (keyState.IsKeyDown(Keys.W))
            { 
                if(myPosition.Y >= 0)
                {
                    tempDir.Y = -1;
                }
            }
            if (keyState.IsKeyDown(Keys.S))
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

            if (keyState.IsKeyDown(Keys.S) || keyState.IsKeyDown(Keys.W) || keyState.IsKeyDown(Keys.D) || keyState.IsKeyDown(Keys.A))
            {
                myPosition += (tempDir * mySpeed);
            }
        }


    }
}
