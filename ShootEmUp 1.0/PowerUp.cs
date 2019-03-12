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
        public int myPowerType;

        public float myDeltaTime;
        Player myPlayer;

        public int myPowerUpType = 0;
        public int myPowerUpIndex = 0;


        public PowerUp(float aSpeed, Texture2D aTexture, Vector2 aStartPos, int aPowerType, Player aPlayer, Game1 aGame)
        {
            mySpeed = aSpeed;
            myTexture = aTexture;
            myPosition = aStartPos;
            myPowerType = aPowerType;
            myDir = new Vector2(0, -1);
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

        public override void Update(GameTime aGameTime)
        {
            Collision();
            myPosition += (myDir * mySpeed);
            myRectangle.Location = (myPosition).ToPoint();
            myDeltaTime = (float)aGameTime.ElapsedGameTime.TotalSeconds;
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
                            (GameState.myGameObjects[i] as Player).myAttackSpeed = 0.1f;
                            GameState.myPowerUpCoolDownSeconds = 5f;
                            GameState.myPowerUpCoolDown = true;
                        }
                        if (myPowerType == 2)
                        {
                            GameState.myPowerUp = "More Speed";
                            (GameState.myGameObjects[i] as Player).mySpeed = 13;
                            GameState.myPowerUpCoolDownSeconds = 10f;
                            GameState.myPowerUpCoolDown = true;
                        }
                        if (myPowerType == 3)
                        {
                            GameState.myPowerUp = "+5 HP";
                            (GameState.myGameObjects[i] as Player).myHp += 5;
                        }
                        GameState.myShowText = true;
                        myPowerUpType = myPowerType;
                        myPowerUpIndex = i;
                        myRemove = true;
                    }
                }
            }
            if (GameState.myPowerUpCoolDown)
            {
                PowerUpTimer(myPowerUpType, myPowerUpIndex);
            }
        }

        public void PowerUpTimer(int aType, int index)
        {
            GameState.myPowerUpCoolDownSeconds -= myDeltaTime;

            if (GameState.myPowerUpCoolDownSeconds <= 0)
            {
                if (aType == 2)
                {
                    myPlayer.mySpeed = 7;
                }
                if (aType == 1)
                {
                    myPlayer.myAttackSpeed = 0.5f;
                }
                GameState.myPowerUpCoolDown = false;
            }
        }
    }
}
