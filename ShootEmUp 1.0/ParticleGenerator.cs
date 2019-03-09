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
    class ParticleGenerator
    {
        Texture2D myTexture;

        float mySpawnWidth;
        float myDensity;
        Vector2 myDir;
        
        List<BackgroundStars> myStars = new List<BackgroundStars>();

        float myDeltaTime;
        float myTimer = 0.2f;
        Random myRng, myRng2;

        public ParticleGenerator(Texture2D aNewTexture, float aNewSpawWidth, float aNewDensity)
        {
            myTexture = aNewTexture;
            mySpawnWidth = aNewSpawWidth;
            myDensity = aNewDensity;
            myDir = new Vector2(0, -7);
            myRng = new Random();
            myRng2 = new Random();

            for (int i = 0; i < 100; i++)
            {
                myStars.Add(new BackgroundStars(myTexture, new Vector2(myRng.Next(1, 701), myRng.Next(1, 901)), myDir));
            }
        }

        public void Update(GameTime aGameTime, GraphicsDevice aGraphics)
        {
            KeyboardState tempKeyBoard = Keyboard.GetState();

            myDeltaTime += (float)aGameTime.ElapsedGameTime.TotalSeconds;

            while(myDeltaTime > 0)
            {
                myDeltaTime -= 1 / myDensity;
                CreateParticle();
            }

            for (int i = 0; i < myStars.Count; i++)
            {
                myStars[i].Update();

                if(myStars[i].accessPosition.Y > aGraphics.Viewport.Height)
                {
                    myStars.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            foreach (BackgroundStars star in myStars)
            {
                star.Draw(aSpriteBatch);
            }
        }

        public void CreateParticle()
        {
            double any = myRng.Next();

            myStars.Add(new BackgroundStars(myTexture, new Vector2(-50 + (float)myRng.NextDouble() * mySpawnWidth, 900), myDir));
        }
    }
}
