using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ShootEmUp_1._0
{
    public class Enemy
    {
        public Texture2D myEnemyTexture;
        public Vector2 myPosition;
        public Rectangle myRectangle;
        Vector2 myOffset;
        Vector2 myDir;
        bool myMoveSelection;
        Vector2 myStartPos;
        Random rng;
        public Vector2 myBulletSpawn;

        int myMove;
        public float myEnemyType;
        float mySpeed;
        float myDeltaTime;
        float myRotation;
        float myScale;

        public Enemy(Texture2D aTexture, float aScale, Vector2 aPosition, float anEnemyType)
        {
            rng = new Random();
            myEnemyTexture = aTexture;
            myPosition = aPosition;
            myScale = aScale;
            myEnemyType = anEnemyType;
            mySpeed = 5;

            myStartPos = aPosition;
            myRectangle = new Rectangle(0, 0, myEnemyTexture.Width, myEnemyTexture.Height);

            myBulletSpawn = new Vector2(myEnemyTexture.Bounds.Size.X * 0.5f, 0);

            if (rng.Next(1,3) == 1)
            {
                myMove = 1;
            }
            else if(rng.Next(1,3) == 2)
            {
                myMove = -1;
            }
            myDir = new Vector2(myMove, -1);
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            if (myEnemyType == 1)
            {
                aSpriteBatch.Draw(myEnemyTexture, myPosition, null, Color.White, myRotation, myOffset, 1f, SpriteEffects.None, 1);
            }
            else if (myEnemyType == 2)
            {
                aSpriteBatch.Draw(myEnemyTexture, myPosition, null, Color.Red, myRotation, myOffset, 1f, SpriteEffects.None, 1);
            }
        }

        public void Update(GameTime aGameTime)
        {
            if(myMoveSelection)
            {
                TypeTwoMove(myStartPos);
            }

            myDeltaTime = (float)aGameTime.ElapsedGameTime.TotalSeconds;

            EnemyTypes();
            myPosition += (myDir * mySpeed);
            myRectangle.Location = myPosition.ToPoint();

        }

        public void EnemyTypes()
        {
            if (myEnemyType == 1)
            {
                myDir.X = 0;
            }
            else if (myEnemyType == 2)
            {
                myMoveSelection = true;
            }
            else if (myEnemyType == 3)
            {

            }
        }

        public void TypeTwoMove(Vector2 aPos)
        {
            if (myPosition.X > 700-myEnemyTexture.Width || myPosition.X > aPos.X + 100)
            {
                myDir.X = -1;
            }
            if (myPosition.X < 0 || myPosition.X < aPos.X - 100)
            {
                myDir.X = 1;
            }
        }
    }
}
