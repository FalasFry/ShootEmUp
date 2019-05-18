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
    class Lazer : GameObject
    {
        List<Bullet> myParts;

        public Lazer(Vector2 aTarget, Texture2D aTexture, Vector2 aStartPos, float aOwner, Color aPaint)
        {
            myParts = new List<Bullet>();
            Vector2 tempPos = aStartPos;

            for (int i = 1; i < 21; i++)
            {
                //tempPos += tempSpace;
                myParts.Add(new Bullet(0, tempPos, aTexture, tempPos, aOwner, aPaint));
                Vector2 tempSpace = new Vector2(aTexture.Width, aTexture.Height) * aTarget;
                tempPos += tempSpace;
            }

            for (int i = 0; i < myParts.Count; i++)
            {
                GameState.myGameObjects.Add(myParts[i]);
            }
        }

        public override void Update(GameTime aGameTime)
        {
            Collision();
        }

        public void Collision()
        {
            for (int i = 0; i < GameState.myGameObjects.Count; i++)
            {
                if (GameState.myGameObjects[i] is Player)
                {
                    for (int j = 0; j < myParts.Count; j++)
                    {
                        if (GameState.myGameObjects[i].myRectangle.Intersects(myParts[j].myRectangle) && myParts[i].myOwner == 2)
                        {
                            for (int k = 0; k < myParts.Count; k++)
                            {
                                myParts[k].myRemove = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
