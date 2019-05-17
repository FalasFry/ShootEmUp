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
        public float myOwner;
        float myAngle;
        float myTimer = 1f;
        Vector2 myOrigin;

        Vector2 v;
        Vector2 v1;
        Vector2 v3;
        Vector2 v4;
        Vector2 p;
        Vector2 p1;
        Vector2 p2;
        Vector2 p3;

        List<Vector2> list1;
        List<Vector2> list2;

        public Lazer(Vector2 aTarget, Texture2D aTexture, Vector2 aStartPos, float aOwner, Color aPaint)
        {
            myOwner = aOwner;
            myDir = aTarget;
            myTexture = aTexture;
            myPosition = aStartPos;
            myOrigin = new Vector2((float)myTexture.Width / 2, (float)myTexture.Height / 2);
            myRectangle = new Rectangle(0, 0, myTexture.Width * (int)myScale, myTexture.Height * (int)myScale);
            myColor = aPaint;
            myDir = (aTarget + new Vector2(0, GameState.myPlayer.myTexture.Height * 0.5f)) - (myPosition + myOffset);
            myDir.Normalize();
            myAngle = (float)Math.Atan2(myDir.X, myDir.Y) * -1;
            myRotation = myAngle;

            v = myPosition.Rotate(myAngle);
            v1 = new Vector2(myPosition.X + myTexture.Width, myPosition.Y).Rotate(myAngle);
            v3 = new Vector2(myPosition.X, myPosition.Y + myTexture.Height).Rotate(myAngle);
            v4 = new Vector2(myPosition.X + myTexture.Width, myPosition.Y + myTexture.Height).Rotate(myAngle);

            p = GameState.myPlayer.myPosition;
            p1 = new Vector2(GameState.myPlayer.myPosition.X + GameState.myPlayer.myTexture.Width, GameState.myPlayer.myPosition.Y);
            p2 = new Vector2(GameState.myPlayer.myPosition.X, GameState.myPlayer.myPosition.Y + GameState.myPlayer.myTexture.Height);
            p3 = new Vector2(GameState.myPlayer.myPosition.X + GameState.myPlayer.myTexture.Width, GameState.myPlayer.myPosition.Y + GameState.myPlayer.myTexture.Height);

            list1 = new List<Vector2>()
            {
                v,
                v1,
                v3,
                v4,
            };
            list2 = new List<Vector2>()
            {
                p,
                p1,
                p2,
                p3,
            };

        }

        public override void Update(GameTime aGameTime)
        {
            Collision(GameState.myPlayer.myHp);
            myTimer -= GameState.myDeltaTime;
            if (myTimer <= 0)
            {
                myRemove = true;
            }

        }

        public void Collision(float aCurrentHP)
        {
            for (int i = 0; i < GameState.myGameObjects.Count; i++)
            {
                if (GameState.myGameObjects[i] is Player)
                {
                    //if (GameState.myGameObjects[i].myRectangle.Intersects(myRectangle) && myOwner == 2)
                    //{
                    //    (GameState.myGameObjects[i] as Player).myHp--;
                    //    myRemove = true;
                    //}

                    //for (int j = 0; j < list1.Count; j++)
                    //{
                    //    for (int k = 0; k < list2.Count; k++)
                    //    {
                    //        if (list2[k].X < list1[k].X || list2[k].Y < list1[k].Y)
                    //        {
                    //            (GameState.myGameObjects[i] as Player).myHp = aCurrentHP--;
                    //            //myRemove = true;
                    //        }
                    //    }
                    //}

                    //if (list2[0].X > list1[0].X && list2[0].X > list1[1].X && list2[0].Y > list1[0].Y && list2[0].Y < list1[1].Y)
                    //{
                    //    (GameState.myGameObjects[i] as Player).myHp = aCurrentHP--;
                    //    //myRemove = true;
                    //}

                }
            }
        }
    }
    public static class Vector2Extension
    {
        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float sin = (float)Math.Sin(degrees);
            float cos = (float)Math.Cos(degrees);

            float tx = v.X;
            float ty = v.Y;
            v.X = (cos * tx) - (sin * ty);
            v.Y = (sin * tx) + (cos * ty);
            return v;
        }
    }

}
