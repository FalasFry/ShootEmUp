using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShootEmUp_1._0
{
    class Bullet : GameObject
    {
        public float myOwner;

        public Bullet(float aSpeed, Vector2 aDir, Texture2D aTexture, Vector2 aStartPos, float aOwner, Color aPaint)
        {
            myOwner = aOwner;
            mySpeed = aSpeed;
            myDir = aDir;
            myTexture = aTexture;
            myPosition = aStartPos;
            myRectangle = new Rectangle(0, 0, myTexture.Width * (int)myScale, myTexture.Height * (int)myScale);
            myColor = aPaint;
        }

        public override void Update(GameTime aGameTime)
        {
            Collision();
            myPosition += (myDir * mySpeed);
            myRectangle.Location = myPosition.ToPoint();
        }

        public void Collision()
        {
            for (int i = 0; i < GameState.myGameObjects.Count; i++)
            {
                if (GameState.myGameObjects[i] is EnemyBase)
                {
                    if (GameState.myGameObjects[i].myRectangle.Intersects(myRectangle) && myOwner == 1)
                    {
                        (GameState.myGameObjects[i] as EnemyBase).myHealth--;
                        myRemove = true;
                    }
                }
                else if (GameState.myGameObjects[i] is Player)
                {
                    if (GameState.myGameObjects[i].myRectangle.Intersects(myRectangle) && myOwner == 2)
                    {
                        (GameState.myGameObjects[i] as Player).myHp--;
                        myRemove = true;
                    }
                }
                else if(GameState.myGameObjects[i] is Wall)
                {
                    if(GameState.myGameObjects[i].myRectangle.Intersects(myRectangle))
                    {
                        (GameState.myGameObjects[i] as Wall).myRemove = true;
                        GameState.myWallsDestroyed++;
                        myRemove = true;
                    }
                }
            }
        }
    }
    class Lazer : GameObject
    {
        public float myOwner;
        Vector2 v1;
        Vector2 v2;
        Vector2 v3;
        Vector2 v4;
        List<Vector2> myCorners;
        public Lazer(Vector2 aDir, Texture2D aTexture, Vector2 aStartPos, float aOwner, Color aPaint)
        {
            myOwner = aOwner;
            myDir = aDir;
            myTexture = aTexture;
            myPosition = aStartPos;
            myRectangle = new Rectangle(0, 0, myTexture.Width * (int)myScale, myTexture.Height * (int)myScale);
            myColor = aPaint;

            v1 = new Vector2(myRectangle.X, myRectangle.Y);
            v2 = new Vector2(myRectangle.X + myRectangle.Width, myRectangle.Y);
            v3 = new Vector2(myRectangle.X, myRectangle.Y + myRectangle.Height);
            v4 = new Vector2(myRectangle.X + myRectangle.Width, myRectangle.Y + myRectangle.Height);

            myCorners = new List<Vector2>()
            {
                v1,
                v2,
                v3,
                v4,
            };
        }

        public override void Update(GameTime aGameTime)
        {
            Collision();
            myPosition += (myDir * mySpeed);
            myRectangle.Location = myPosition.ToPoint();

            for (int i = 0; i < myCorners.Count; i++)
            {
                myCorners[i] = Vector2.Transform(myCorners[i], Matrix.CreateRotationZ(MathHelper.ToRadians(30)));
            }

        }
        public void Collision()
        {
            for (int i = 0; i < GameState.myGameObjects.Count; i++)
            {
                if (GameState.myGameObjects[i] is EnemyBase)
                {
                    if (GameState.myGameObjects[i].myRectangle.Intersects(myRectangle) && myOwner == 1)
                    {
                        (GameState.myGameObjects[i] as EnemyBase).myHealth--;
                        myRemove = true;
                    }
                }
                else if (GameState.myGameObjects[i] is Player)
                {
                    if (GameState.myGameObjects[i].myRectangle.Intersects(myRectangle) && myOwner == 2)
                    {
                        (GameState.myGameObjects[i] as Player).myHp--;
                        myRemove = true;
                    }
                }
                else if (GameState.myGameObjects[i] is Wall)
                {
                    if (GameState.myGameObjects[i].myRectangle.Intersects(myRectangle))
                    {
                        (GameState.myGameObjects[i] as Wall).myRemove = true;
                        GameState.myWallsDestroyed++;
                        myRemove = true;
                    }
                }
            }
        }
    }
}
