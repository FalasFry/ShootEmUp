using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootEmUp_1._0
{
    class WeaponPowerUp : GameObject
    {
        public static int myPowerType;

        Player myPlayer;

        public static int myPowerUpIndex = 0;

        public WeaponPowerUp(float aSpeed, Texture2D aTexture, Vector2 aStartPos, int aPowerType, Player aPlayer, Game1 aGame)
        {
            mySpeed = aSpeed;
            myTexture = aTexture;
            myPosition = aStartPos;
            myPowerType = aPowerType;
            myDir = new Vector2(0, -1);
            myRectangle = new Rectangle(0, 0, myTexture.Width * (int)myScale, myTexture.Height * (int)myScale);
        }

        public override void Update(GameTime aGameTime)
        {
            Collision();

            myPosition += (myDir * mySpeed);
            myRectangle.Location = (myPosition).ToPoint();
        }

        public void Collision()
        {
            for (int i = 0; i < GameState.myGameObjects.Count; i++)
            {
                if (GameState.myGameObjects[i] is Player)
                {
                    myPlayer = (Player)GameState.myGameObjects[i];

                    if (GameState.myGameObjects[i].myRectangle.Intersects(myRectangle))
                    {
                        if (myPowerType >= 1 && myPowerType <= 50)
                        {
                            GameState.myPowerUp = "All At Once";
                            Random tempRng = new Random();
                            myPlayer.myFireType = tempRng.Next(1, 3);
                            myPlayer.myNormalFire = false;

                        }
                        if (myPowerType > 50 && myPowerType <= 65)
                        {
                            GameState.myPowerUp = "Ultimate Ready";
                            myPlayer.myUltimate = true;
                        }
                        if (myPowerType > 65 && myPowerType <= 100)
                        {
                            if (myPlayer.myAttackSpeed >= 0.1)
                            {
                                GameState.myPowerUp = "+FireRate";
                                myPlayer.myAttackSpeed -= 0.1f;
                                myPlayer.myBaseAttackSpeed = myPlayer.myAttackSpeed;
                            }
                            else
                            {
                                GameState.myPowerUp = "Already Maxed";
                                myPlayer.myAttackSpeed = myPlayer.myBaseAttackSpeed;
                            }
                        }
                        GameState.myPowerUpCount++;
                        GameState.myShowText = true;
                        myPowerUpIndex = i;
                        myRemove = true;
                        return;
                    }
                }
            }

        }
    }
}
