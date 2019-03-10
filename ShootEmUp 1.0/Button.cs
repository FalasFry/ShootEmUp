using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShootEmUp_1._0
{
    public class Button : Components
    {
        Texture2D myTexture;
        SpriteFont myFont;
        bool myIsHovering;
        MouseState myPrevMouse;
        MouseState myCurMouse;

        public event EventHandler Click;
        public bool AccessClicked { get; set; }
        public Color AccessPaint { get; set; }
        public Vector2 AccessPos { get; set; }
        public string AccessText { get; set; }
        public Rectangle AccessRectangle
        {
            get
            {
                return new Rectangle((int)AccessPos.X, (int)AccessPos.Y, myTexture.Width, myTexture.Height);
            }
        }

        public Button(SpriteFont aFont, Texture2D aTexture)
        {
            myTexture = aTexture;
            myFont = aFont;
            AccessPaint = Color.Black;
        }

        public override void Draw(GameTime aGameTime, SpriteBatch aSpriteBatch)
        {
            Color tempColor = Color.White;

            if(myIsHovering)
            {
                tempColor = Color.Gray;
            }

            aSpriteBatch.Draw(myTexture, AccessRectangle, tempColor);

            if(!string.IsNullOrEmpty(AccessText))
            {
                float tempX = AccessRectangle.X + (AccessRectangle.Width / 2) - (myFont.MeasureString(AccessText).X / 2);
                float tempY = AccessRectangle.Y + (AccessRectangle.Height / 2) - (myFont.MeasureString(AccessText).Y / 2);

                aSpriteBatch.DrawString(myFont, AccessText, new Vector2(tempX, tempY), AccessPaint);
            }
        }

        public override void Update(GameTime aGameTime)
        {
            myIsHovering = false;

            myPrevMouse = myCurMouse;
            myCurMouse = Mouse.GetState();

            Rectangle tempRect = new Rectangle(myCurMouse.X, myCurMouse.Y, 1, 1);

            if(tempRect.Intersects(AccessRectangle))
            {
                myIsHovering = true;
                if (myCurMouse.LeftButton == ButtonState.Released && myPrevMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }
    }
}
