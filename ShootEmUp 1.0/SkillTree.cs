using Microsoft.Xna.Framework;
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
                mySplitted = myText[0].Split(':');

                myUnlockSupers = Convert.ToBoolean(mySplitted[1]);
                mySpeedMult = Convert.ToInt32(mySplitted[3]);
                myFirerateMult = Convert.ToInt32(mySplitted[5]);
                mySlowerEnemiesMult = Convert.ToInt32(mySplitted[7]);
                myHealthUpgrade = Convert.ToInt32(mySplitted[9]);
                
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
            }
        }

        public static void End()
        {
            string[] tempArray =
            {
                "Unlock SuperPowerups:" +GameState.mySuperPowerUpUnlocked,
                "Upgrade Speed:" +mySpeedMult,
                "Upgrade Firerate:" +myFirerateMult,
                "Slowe enemies:" +mySlowerEnemiesMult,
                "More health:" +myHealthUpgrade
            };
            File.WriteAllLines(myPath, tempArray);
        }
    }
}

