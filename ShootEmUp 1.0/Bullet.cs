using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShootEmUp_1
{
    public class Bullet
    {
        public float mySpeed, myRotation = (float)(1.5f * Math.PI);
        public Vector2 myDir, myPosition, myOffset, myScale = new Vector2(1, 1);
        public Texture2D myTexture;
        public Rectangle myRectangle;
        public float myDamage = 1;
        public Color myColor;
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

        public void Update()
        {
            myPosition += (myDir * mySpeed);
            myRectangle.Location = (myPosition - (myRectangle.Size.ToVector2() * 0.5f)).ToPoint();
        }

        public void DrawBullet(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(myTexture, myPosition, null, myColor, myRotation, myOffset, 1f, SpriteEffects.None, 1);
        }
    }
}
