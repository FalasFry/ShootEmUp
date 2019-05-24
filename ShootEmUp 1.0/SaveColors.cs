using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootEmUp_1._0
{
    class SaveColors
    {
        static string myPath;
        string[] myText;
        string[] mySplitted;
        public static int mySelected;

        public SaveColors()
        {
            myPath = Path.GetFullPath("Custom.txt");

            if (File.Exists(myPath))
            {
                string[] tempLines = File.ReadAllLines(myPath);
                myText = tempLines;

                if (myText.Contains("First True"))
                {
                    CustomizeState.myFirst = true;
                }
                if (myText.Contains("Second True"))
                {
                    CustomizeState.mySecond = true;
                }
                if (myText.Contains("Third True"))
                {
                    CustomizeState.myThird = true;
                }

                mySplitted = tempLines[3].Split(':');
                mySelected = Convert.ToInt32(mySplitted[1]);
            }
        }

        public static void End()
        {
            string[] tempArray =
            {   
                "First " +CustomizeState.myFirst,
                "Second " +CustomizeState.mySecond,
                "Third " +CustomizeState.myThird,
                "Selected:" +CustomizeState.mySelected,
            };
            File.WriteAllLines(myPath, tempArray);
        }
    }
}
