using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShootEmUp_1._0
{
    public abstract class Components
    {

        public abstract void Update(GameTime aGameTime);

        public abstract void Draw(GameTime aGameTime, SpriteBatch aSpriteBatch);

    }
}
