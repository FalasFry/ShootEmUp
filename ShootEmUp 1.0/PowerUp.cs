using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootEmUp_1._0
{
    class PowerUp : GameObject
    {
        public static int myPowerType;

        Player myPlayer;

        public static int myPowerUpIndex = 0;


        public PowerUp(float aSpeed, Texture2D aTexture, Vector2 aStartPos, int aPowerType, Player aPlayer, Game1 aGame)
        {
            mySpeed = aSpeed;
            myTexture = aTexture;
            myPosition = aStartPos;
            myPowerType = aPowerType;
            myDir = new Vector2(0, -1);
            myRectangle = new Rectangle((myOffset - myPosition).ToPoint(), (myTexture.Bounds.Size.ToVector2()).ToPoint());
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
                        if (myPowerType == 1)
                        {
                            GameState.myPowerUp = "More AttackSpeed";
                            myPlayer.myAttackSpeed = 0.1f;
                            GameState.myPowerUpCoolDownSeconds = 5f;
                            GameState.myPowerUpCoolDown = true;
                        }
                        if (myPowerType == 2)
                        {
                            GameState.myPowerUp = "More Speed";
                            myPlayer.mySpeed = 13;
                            GameState.myPowerUpCoolDownSeconds = 10f;
                            GameState.myPowerUpCoolDown = true;
                        }
                        if (myPowerType == 3)
                        {
                            GameState.myPowerUp = "+10 HP";
                            (GameState.myGameObjects[i] as Player).myHp += 10;
                        }
                        GameState.myShowText = true;
                        myPowerUpIndex = i;
                        myRemove = true;
                    }
                }
            }

        }
    }
}
