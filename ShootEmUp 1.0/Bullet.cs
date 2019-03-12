using System;
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
            myOffset = ((myTexture.Bounds.Size.ToVector2()) / 2);
            myRectangle = new Rectangle((myOffset - myPosition).ToPoint(), new Point(20, 20));
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
                        myRemove = true;
                        (GameState.myGameObjects[i] as EnemyBase).myHealth--;
                    }
                }
            }
            for (int i = 0; i < GameState.myGameObjects.Count; i++)
            {
                if (GameState.myGameObjects[i] is Player)
                {
                    if (GameState.myGameObjects[i].myRectangle.Intersects(myRectangle) && myOwner == 2)
                    {
                        myRemove = true;
                        (GameState.myGameObjects[i] as Player).myHp--;
                    }
                }
            }
        }
    }
}
