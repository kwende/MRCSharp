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
            Map map = new Map();
            map.Build(100, 100, 1000); 
        }
    }
}
