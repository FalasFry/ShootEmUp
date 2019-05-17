using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootEmUp_1._0
{
    abstract class GameObject
    {
        public float mySpeed, myRotation;
        public Vector2 myDir, myPosition, myOffset;
        public Texture2D myTexture;
        public Rectangle myRectangle;
        public float myDamage = 1;
        public Color myColor = Color.White;
        public float myScale = 1;
        public bool myRemove = false;
        float myTimer;
        float myStartTimr;

        public GameObject()
        {
            myTimer = 0.5f;
            myStartTimr = myTimer;
        }

        public abstract void Update(GameTime aGameTime);

        public void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(myTexture, myPosition, null, myColor, myRotation, myOffset, myScale, SpriteEffects.None, 1);
            aSpriteBatch.Draw(myTexture, myRectangle, Color.Pink);
        }

        public void Animation(List<Texture2D> aList)
        {
                myTimer -= GameState.myDeltaTime;

                if (myTimer <= 0)
                {
                    myTimer = myStartTimr;
                    for (int i = 0; i < aList.Count; i++)
                    {
                        if (myTexture == aList[i])
                        {
                            if (i < aList.Count - 1)
                            {
                                myTexture = aList[i + 1];
                                return;
                            }
                            else if (i >= aList.Count - 1)
                            {
                                myTexture = aList[0];
                                return;
                            }
                        }
                    }
                }
            
        }
    }
}
