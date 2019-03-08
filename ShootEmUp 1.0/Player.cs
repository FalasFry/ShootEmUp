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
    public class Player
    {
        public float mySpeed = 10;
        public float myRotation = 0;
        public float myAmmo = 20;
        public float myHp = 5;
        public float myAttackSpeed = 0.5f;
        public float myAttackTimer = 0;
        public bool myDead = false;

        public Vector2 myPosition = new Vector2(10,10);
        public Vector2 myOffset;
        public Texture2D myTexture;
        public Rectangle myRectangle;
        Game1 myGame1;
        public Vector2 myPrevPos;
        public Vector2 myBulletsSpawn;


        public Player(Game1 aGame, Texture2D aTexture)
        {
            myTexture = aTexture;

            myGame1 = aGame;
            myOffset = ((myTexture.Bounds.Size.ToVector2() * 0.5f));
            myBulletsSpawn = new Vector2(myTexture.Bounds.Size.X * 0.5f, myTexture.Bounds.Size.Y);
            myPosition = new Vector2((350-myTexture.Width), 0);
            myRectangle = new Rectangle((myOffset).ToPoint(), (aTexture.Bounds.Size.ToVector2()).ToPoint());

        }


        public void Update(GameTime aGameTime)
        {
            Movement();
        }

        // Makes it so you move with wasd.
        public void Movement()
        {
            
            KeyboardState keyState = Keyboard.GetState();
            Vector2 tempDir = new Vector2();

            if (keyState.IsKeyDown(Keys.A))
            {
                if(myPosition != new Vector2(0, myPosition.Y))
                {
                    tempDir.X = -1;
                }
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                if(myPosition != new Vector2((700 - myTexture.Width), myPosition.Y))
                {
                    tempDir.X = 1;
                }

            }
            if (keyState.IsKeyDown(Keys.W))
            { 
                if(myPosition != new Vector2(myPosition.X, 0))
                {
                    tempDir.Y = -1;
                }

            }
            if (keyState.IsKeyDown(Keys.S))
            {
                if (myPosition != new Vector2(myPosition.X, ((900/2) - myTexture.Height)))
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

        public void Draw(SpriteBatch aSpriteBatch)
        {

            if (!myDead)
            {
                aSpriteBatch.Draw(myTexture, myPosition + myOffset, null, Color.White, myRotation, myOffset, 1f, SpriteEffects.None, 0);
            }

        }
    }
}
