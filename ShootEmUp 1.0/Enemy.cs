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
        Vector2 myPosition;
        public Rectangle myRectangle;
        float myRotation;
        float myScale;
        Vector2 myOffset;
        float myEnemyType;
        float mySpeed;
        Vector2 myDir;
        bool myMoveSelection;
        Vector2 myStartPos;
        Random rng;

        public Enemy(Texture2D aTexture, float aScale, Vector2 aPosition, float anEnemyType)
        {
            rng = new Random();
            myEnemyTexture = aTexture;
            myPosition = aPosition;
            myScale = aScale;
            myEnemyType = anEnemyType;
            mySpeed = 5;
            myDir = new Vector2(1, -1);
            myStartPos = aPosition;
            myRectangle = new Rectangle(0, 0, myEnemyTexture.Width, myEnemyTexture.Height);
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(myEnemyTexture, myPosition, null, Color.White, myRotation, myOffset, 1f, SpriteEffects.None, 1);
        }

        public void Update(GameTime aGameTime)
        {
            if(myMoveSelection)
            {
                TypeTwoMove(myStartPos);
            }

            EnemyTypes();
            myPosition += (myDir * mySpeed);
            myRectangle.Location = myPosition.ToPoint();
        }

        public void EnemyTypes()
        {
            if(rng.Next(1,3)== 1)
            {
                myDir.X = 1;
            }
            else if(rng.Next(1,3)== 2)
            {
                myDir.X = -1;
            }

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
            if (myPosition.X > 700-myEnemyTexture.Width)
            {
                myDir.X = -1;
            }
            if (myPosition.X < 0)
            {
                myDir.X = 1;
            }
        }


    }
}
