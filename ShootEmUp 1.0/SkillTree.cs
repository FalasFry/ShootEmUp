﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootEmUp_1._0
{
    class SkillTree
    {
        static string myPath;
        string[] myText;
        string[] mySplitted;

        public static bool myUnlockSupers;
        public static int mySpeedMult;
        public static int myFirerateMult;
        public static int mySlowerEnemiesMult;
        public static int myHealthUpgrade;

        public SkillTree()
        {

            myPath = Path.GetFullPath("Skills.txt");

            if (File.Exists(myPath))
            {
                string[] tempLines = File.ReadAllLines(myPath);
                myText = tempLines;
                mySplitted = new string[myText.Count()];

                for (int i = 0; i < myText.Count(); i++)
                {
                    mySplitted[i] = myText[i].Split(':')[1];
                }


                myUnlockSupers = Convert.ToBoolean(mySplitted[0]);
                mySpeedMult = Convert.ToInt32(mySplitted[1]);
                myFirerateMult = Convert.ToInt32(mySplitted[2]);
                mySlowerEnemiesMult = Convert.ToInt32(mySplitted[4]);
                myHealthUpgrade = Convert.ToInt32(mySplitted[4]);

            }
            else if(!File.Exists(myPath))
            {
                string[] tempText =
                {
                    "Unlock SuperPowerups:false",
                    "Upgrade Speed:0",
                    "Upgrade Firerate:0",
                    "Slower enemies:0",
                    "More health:0"
                };
                File.WriteAllLines(myPath, tempText);
            }
        }

        public static void End()
        {
            string[] tempArray =
            {
                "Unlock SuperPowerups:" +myUnlockSupers,
                "Upgrade Speed:" +mySpeedMult,
                "Upgrade Firerate:" +myFirerateMult,
                "Slowe enemies:" +mySlowerEnemiesMult,
                "More health:" +myHealthUpgrade
            };
            File.WriteAllLines(myPath, tempArray);
        }
    }
}

