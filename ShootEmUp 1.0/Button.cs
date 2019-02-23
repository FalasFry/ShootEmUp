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
    class Button : Components
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

        public Button(SpriteFont Font, Texture2D Texture)
        {
            myTexture = Texture;
            myFont = Font;
            AccessPaint = Color.Black;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Color colour = Color.White;

            if(myIsHovering)
            {
                colour = Color.Gray;
            }

            spriteBatch.Draw(myTexture, AccessRectangle, colour);

            if(!string.IsNullOrEmpty(AccessText))
            {
                float x = AccessRectangle.X + (AccessRectangle.Width / 2) - (myFont.MeasureString(AccessText).X / 2);
                float y = AccessRectangle.Y + (AccessRectangle.Height / 2) - (myFont.MeasureString(AccessText).Y / 2);

                spriteBatch.DrawString(myFont, AccessText, new Vector2(x, y), AccessPaint);
            }
        }

        public override void Update(GameTime gameTime)
        {
            myIsHovering = false;

            myPrevMouse = myCurMouse;
            myCurMouse = Mouse.GetState();

            Rectangle mRect = new Rectangle(myCurMouse.X, myCurMouse.Y, 1, 1);

            if(mRect.Intersects(AccessRectangle))
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
