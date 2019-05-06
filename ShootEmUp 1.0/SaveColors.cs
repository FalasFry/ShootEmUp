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
        StreamReader myReader;
        static string myPath;
        string[] myText;

        public SaveColors()
        {
            myPath = Path.GetFullPath("test.txt");

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
            }
        }

        public static void End()
        {
            List<string> tempLines = new List<string>();

            if(CustomizeState.myFirst)
            {
                tempLines.Add("First True");
            }
            if (CustomizeState.mySecond)
            {
                tempLines.Add("Second True");
            }
            if (CustomizeState.myThird)
            {
                tempLines.Add("Third True");
            }
            File.WriteAllLines(myPath, tempLines.ToArray());

            if (tempLines.Count == 0)
            {
                string[] tempArray = { "First False", "Second False", "Third False" };

                File.WriteAllLines(myPath, tempArray);
            }
        }
    }
}
