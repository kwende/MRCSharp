using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random(1234);
            for (int c = 0; c < 1000; c++)
            {
                Console.WriteLine(c.ToString()); 
                Tomogram tom = TomogramBuilder.BuildTomogram(860, 934, 100000, rand.Next(5, 25));
                TomogramBuilder.SaveAsBitmap(tom, $"C:/users/ben/desktop/toms/{c}.bmp");
                TomogramBuilder.SaveAsDatFile(tom, $"C:/users/ben/desktop/toms/{c}.dat");
            }
        }
    }
}
