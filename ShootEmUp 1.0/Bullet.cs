using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShootEmUp_1._0
{
    class Bullet : GameObject
    {
        public float myOwner;
        bool isLaser;

        public Bullet(float aSpeed, Vector2 aDir, Texture2D aTexture, Vector2 aStartPos, float aOwner, Color aPaint, bool aSeeAsLaser)
        {
            myOwner = aOwner;
            mySpeed = aSpeed;
            myDir = aDir;
            myTexture = aTexture;
            myPosition = aStartPos;
            myRectangle = new Rectangle(0, 0, myTexture.Width * (int)myScale, myTexture.Height * (int)myScale);
            myColor = aPaint;
            isLaser = aSeeAsLaser;
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
                    if (GameState.myGameObjects[i].myRectangle.Intersects(myRectangle) && myOwner == 2 && !isLaser)
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
        float myRemoveTime;
        bool myDelete;
        float myHP;

        public Lazer(Vector2 aTarget, Texture2D aTexture, Vector2 aStartPos, float aOwner, Color aPaint)
        {
            myParts = new List<Bullet>();
            Vector2 tempPos = aStartPos;
            myTexture = aTexture;
            myPosition = new Vector2(0, 900+myTexture.Height);
            myRemoveTime = 1;
            myDelete = false;
            myHP = GameState.myPlayer.myHp;
            for (int i = 1; i < 51; i++)
            {
                myParts.Add(new Bullet(0, tempPos, aTexture, tempPos, aOwner, aPaint, true));
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
            Collision(myHP);
            myRemoveTime -= GameState.myDeltaTime;
            if (myRemoveTime <= 0)
            {
                for (int k = 0; k < myParts.Count; k++)
                {
                    myParts[k].myRemove = true;
                }
                myRemove = true;
            }
        }

        public void Collision(float aCurHealth)
        {
            for (int i = 0; i < GameState.myGameObjects.Count; i++)
            {
                if (GameState.myGameObjects[i] is Player)
                {
                    for (int j = 0; j < myParts.Count; j++)
                    {
                        if (GameState.myGameObjects[i].myRectangle.Intersects(myParts[j].myRectangle))
                        {
                            (GameState.myGameObjects[i] as Player).myHp = aCurHealth -1;
                        }
                    }
                }
            }
        }
    }
}
