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
        public Color myColor = Color.White;
        public float myScale = 1;
        public bool myRemove = false;
        protected float myTimer;
        protected float myStartTimer;

        public GameObject()
        {
            myTimer = 0.5f;
            myStartTimer = myTimer;
        }

        public abstract void Update(GameTime aGameTime);

        public void Draw(SpriteBatch aSpriteBatch)
        {
            //aSpriteBatch.Draw(GameState.myEnemyLazer, myRectangle, Color.Pink);
            aSpriteBatch.Draw(myTexture, myPosition, null, myColor, myRotation, myOffset, myScale, SpriteEffects.None, 1);
        }

        public void Animation(List<Texture2D> aList)
        {
                myTimer -= GameState.myDeltaTime;

                if (myTimer <= 0)
                {
                    myTimer = myStartTimer;
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
