using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ShootEmUp_1._0
{
    class HighScore
    {
        StreamReader myReader;

        public HighScore()
        {
            string myPath = Path.GetFullPath("/Content/test.txt");
            string[] myLine = File.ReadAllLines(myPath);

            if(myLine.Contains("First True"))
            {
                CustomizeState.myFirst = true;
            }
            if (myLine.Contains("Second True"))
            {
                CustomizeState.mySecond = true;
            }
            if (myLine.Contains("Third True"))
            {
                CustomizeState.myThird = true;
            }
        }
    }
}
