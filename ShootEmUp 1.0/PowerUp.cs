using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootEmUp_1._0
{
    public class PowerUp
    {
        float mySpeed;
        float myRotation;
        Vector2 myDir = new Vector2(0, -1);
        Vector2 myPosition;
        Vector2 myOffset;
        Texture2D myTexture;
        public Rectangle myRectangle;
        Color myColor;
        public int myPowerType;
        Player myPlayer;
        Game1 myGame;
        public float myCoolDownSeconds;

        float myDeltaTime;
        public bool myCoolDown;

        public PowerUp(float aSpeed, Texture2D aTexture, Vector2 aStartPos, int aPowerType, Player aPlayer, Game1 aGame)
        {
            mySpeed = aSpeed;
            myTexture = aTexture;
            myPosition = aStartPos;
            myPowerType = aPowerType;
            myPlayer = aPlayer;
            myGame = aGame;
            myOffset = ((myTexture.Bounds.Size.ToVector2()) / 2);
            myRectangle = new Rectangle((myOffset - myPosition).ToPoint(), (myTexture.Bounds.Size.ToVector2()).ToPoint());

            if (myPowerType == 1)
            {
                myColor = Color.Green;
            }
            if (myPowerType == 2)
            {
                myColor = Color.Yellow;
            }
            if (myPowerType == 3)
            {
                myColor = Color.Red;
            }
        }
        public void Update(GameTime gameTime)
        {
            myPosition += (myDir * mySpeed);
            myRectangle.Location = (myPosition).ToPoint();
            myDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(myTexture, myPosition + myOffset, null, myColor, myRotation, myOffset, 1f, SpriteEffects.None, 1);
        }
    }
}
