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
            Tomogram tom = TomogramBuilder.BuildTomogram(800, 800, 10000, 5);
            TomogramBuilder.SaveAsBitmap(tom, "C:/users/ben/desktop/fart.bmp"); 
        }
    }
}
