using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootEmUp_1._0
{
    class Player
    {
        public float mySpeed = 5;
        public float myRotation = 0;
        public float myAmmo = 20;
        public float myHp = 5;
        public float myAttackSpeed = 0.5f;
        public float myAttackTimer;
        public bool myDead = false;

        public Vector2 myPosition;
        public Vector2 myOffset;
        public Texture2D myTexture;
        public Rectangle myRectangle;
        Game1 myGame1;


        public Player(Game aGame, Texture2D aTexture)
        {
            myTexture = aTexture;
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
                tempDir.X = -1;

            }
            if (keyState.IsKeyDown(Keys.D))
            {
                tempDir.X = 1;

            }
            if (keyState.IsKeyDown(Keys.W))
            {
                tempDir.Y = -1;

            }
            if (keyState.IsKeyDown(Keys.S))
            {
                tempDir.Y = 1;

            }
            if (tempDir.X > 1f || tempDir.Y > 1f)
            {
                tempDir.Normalize();
            }

            if (tempDir == Vector2.Zero)
            {
                tempDir = new Vector2(1, 0);
            }
            if (keyState.IsKeyDown(Keys.S) || keyState.IsKeyDown(Keys.W) || keyState.IsKeyDown(Keys.D) || keyState.IsKeyDown(Keys.A))
            {
                myPosition += (tempDir * mySpeed);
            }

            myRotation = (float)Math.Atan2(tempDir.X, tempDir.Y) * -1;

        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Begin();

            if (!myDead)
            {
                aSpriteBatch.Draw(myTexture, myPosition + myOffset, null, Color.White, myRotation, myOffset, 1f, SpriteEffects.None, 0);
            }

            aSpriteBatch.End();
        }
    }
}
