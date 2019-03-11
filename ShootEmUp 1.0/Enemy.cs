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
    public class EnemyEasy : EnemyBasic
    {

        public EnemyEasy(Texture2D aTexture, Vector2 aPosition) : base(aTexture)
        {
            myPosition = aPosition;
            mySpeed = 5;
            myStartPos = myPosition;
            myDir = new Vector2(0, -1);
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(myEnemyTexture, myPosition, null, Color.White, myRotation, myOffset, 1f, SpriteEffects.None, 1);
        }

        public override void Update(GameTime aGameTime)
        {
            myDeltaTime = (float)aGameTime.ElapsedGameTime.TotalSeconds;

            myPosition += (myDir * mySpeed);
            myRectangle.Location = myPosition.ToPoint();
        }
    }

    class EnemyMoving : EnemyBasic
    {
        public EnemyMoving(Texture2D aTexture, Vector2 aPosition) : base(aTexture)
        {
            myRng = new Random();
            myEnemyTexture = aTexture;
            myPosition = aPosition;
            mySpeed = 5;

            myStartPos = myPosition;

            if (myRng.Next(1, 3) == 1)
            {
                myMove = 1;
            }
            else if (myRng.Next(1, 3) == 2)
            {
                myMove = -1;
            }
            myDir = new Vector2(myMove, -1);
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(myEnemyTexture, myPosition, null, Color.Red, myRotation, myOffset, 1f, SpriteEffects.None, 1);
        }

        public override void Update(GameTime aGameTime)
        {

            TypeTwoMove(myStartPos);
            
            myPosition += (myDir * mySpeed);
            myRectangle.Location = myPosition.ToPoint();
        }

        public void TypeTwoMove(Vector2 aPos)
        {
            if (myPosition.X > 700 - myEnemyTexture.Width || myPosition.X > aPos.X + 100)
            {
                myDir.X = -1;
            }
            if (myPosition.X < 0 || myPosition.X < aPos.X - 100)
            {
                myDir.X = 1;
            }
        }
    }
    class EnemyBoss : EnemyBasic
    {
        public EnemyBoss(Texture2D aTexture) : base(aTexture)
        {
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime aGameTime)
        {
            throw new NotImplementedException();
        }
    }
}
