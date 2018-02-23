using MRCSharpLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Tester
{
    public static class EdgeTracer
    {
        private class Offset
        {
            public int OffsetX { get; set; }
            public int OffsetY { get; set; }
            public int Difference { get; set; }
        }

        private static List<Offset> GenerateOffsetsForPoint(Bitmap bmp, int x, int y, 
            int numberOfOffset, Random rand)
        {
            for (int a = 0; a < numberOfOffset; a++)
            {

            }
        }


        public static void DoIt()
        {
            using (Bitmap bmp = (Bitmap)Image.FromFile("C:/users/ben/desktop/236.png"))
            {
                int x = 330, y = 365;

                Color color = bmp.GetPixel(x, y);
                bool[] visited = new bool[bmp.Width * bmp.Height];

                int visitedIndex = y * bmp.Width + x;
                visited[visitedIndex] = true;

                for (int a = 0; a < 500; a++)
                {
                    Console.WriteLine(a);
                    int lowestIndex = 0;
                    int lowestGray = int.MaxValue;

                    for (int y1 = y - 1; y1 <= y + 1; y1++)
                    {
                        for (int x1 = x - 1; x1 <= x + 1; x1++)
                        {
                            int index = y1 * bmp.Width + x1; ;

                            if (x != x1 && y != y1 &&
                                x1 < bmp.Width && y1 < bmp.Height && x1 >= 0 &&
                                y1 >= 0)
                            {
                                if (!visited[index])
                                {
                                    Color c = bmp.GetPixel(x1, y1);

                                    if (c.G < lowestGray)
                                    {
                                        lowestGray = c.G;
                                        lowestIndex = index;
                                    }
                                }
                            }
                        }
                    }

                    visited[lowestIndex] = true;
                    x = lowestIndex % bmp.Width;
                    y = lowestIndex / bmp.Width;
                }

                for (int a = 0; a < visited.Length; a++)
                {
                    if (visited[a])
                    {
                        int y1 = a / bmp.Width;
                        int x1 = a % bmp.Width;

                        bmp.SetPixel(x1, y1, Color.Red);
                    }
                }

                bmp.Save("C:/users/ben/desktop/processed.bmp");
            }

            //const string TomogramDirectory = @"C:\Users\Ben\Desktop\tomograms";

            //Console.WriteLine("Loading main file..");
            //MRCFile file = MRCParser.Parse(Path.Combine(TomogramDirectory, "tomography2_fullsirtcliptrim.mrc"));

            //MRCFrame frame = file.Frames[236]; 
        }
    }
}
