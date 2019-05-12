using Microsoft.Xna.Framework;
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
                    }
                    else if (mySplitted[1] == "black")
                    {
                        Game1.myColor = Color.Black;
                    }
                }
            }
        }
        public static void End()
        {
            string[] tempWrite = {""};

            if (!File.Exists(myPath))
            {
               tempWrite[0] = "Background:black";
               File.WriteAllLines(myPath, tempWrite);
            }

        }
    }
}
