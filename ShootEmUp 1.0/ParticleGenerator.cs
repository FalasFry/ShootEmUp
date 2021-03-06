﻿using System;
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
        public static Color myColor = Color.White;
        
        List<BackgroundStars> myStars = new List<BackgroundStars>();

        float myTime;
        Random myRng, myRng2;

        public ParticleGenerator(Texture2D aNewTexture, float aNewSpawWidth, float aNewDensity)
        {
            myTexture = aNewTexture;
            mySpawnWidth = aNewSpawWidth;
            myDensity = aNewDensity;
            myDir = new Vector2(0, -4);
            myRng = new Random();
            myRng2 = new Random();

            for (int i = 0; i < 100; i++)
            {
                int tempRng = myRng2.Next(1,4);
                if(tempRng == 1)
                {
                    myDir.Y = -3;
                }
                if(tempRng == 2)
                {
                    myDir.Y = -5;
                }

                myStars.Add(new BackgroundStars(myTexture, new Vector2(myRng.Next(1, 701), myRng.Next(1, 901)), myDir, myColor, myRng.Next(1,3)));
            }
        }

        public void Update(GameTime aGameTime, GraphicsDevice aGraphics)
        {
            KeyboardState tempKeyBoard = Keyboard.GetState();

            myTime += (float)aGameTime.ElapsedGameTime.TotalSeconds;

            while(myTime > 0)
            {
                myTime -= 1 / myDensity;
                CreateParticle();
            }

            for (int i = 0; i < myStars.Count; i++)
            {
                myStars[i].Update();

                if(myStars[i].accessPosition.Y < 0)
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
            int tempSpeed = myRng.Next(1, 4);
            if(tempSpeed == 1)
            {
                myDir.Y = -3;
            }
            else if(tempSpeed == 2)
            {
                myDir.Y = -5;
            }
            myStars.Add(new BackgroundStars(myTexture, new Vector2(-50 + (float)myRng.NextDouble() * mySpawnWidth, 900), myDir, myColor, myRng2.Next(1,3)));
        }
    }
}
