﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootEmUp_1._0
{
    class MapEditor
    {
        static string myPath;
        string[] myText;
        string[] mySplitted;
        
        public MapEditor()
        {
            myPath = Path.GetFullPath("Map.txt");

            if (File.Exists(myPath))
            {
                string[] tempLines = File.ReadAllLines(myPath);
                myText = tempLines;

                mySplitted = myText[0].Split(':');

                if (mySplitted[0] == "Background")
                {
                    if (mySplitted[1] == "blue")
                    {
                        Game1.myColor = Color.CornflowerBlue;
                        ParticleGenerator.myColor = Color.Yellow;
                    }
                    else if (mySplitted[1] == "black")
                    {
                        Game1.myColor = Color.Black;
                        ParticleGenerator.myColor = Color.White;
                    }
                    else if(mySplitted[1] == "red")
                    {
                        Game1.myColor = Color.Black;
                        ParticleGenerator.myColor = Color.Red;
                    }
                }
            }
            else if(!File.Exists(myPath))
            {
                Game1.myColor = Color.Black;
            }
        }

        public static void End()
        {
            string[] tempWrite = {""};

            if (!File.Exists(myPath))
            {
               tempWrite[0] = "Background:black";
            }
            else if(File.Exists(myPath))
            {
                tempWrite[0] = "Background:" + Game1.myColor;
            }
            File.WriteAllLines(myPath, tempWrite);
        }
    }
}
