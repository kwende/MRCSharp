using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSimulator
{
    public class Map
    {
        public void Build(int width, int height, int numberOfObjects)
        {
            Random rand = new Random(1234);

            int[] map = new int[width * height];

            Dictionary<int, List<int>> lookup = new Dictionary<int, List<int>>();

            // initialize
            for (int p = 1; p <= numberOfObjects; p++)
            {
                int x = rand.Next(0, width);
                int y = rand.Next(0, height);
                int index = y * width + x;

                map[index] = p;
                List<int> list = new List<int>();
                list.Add(index);
                lookup.Add(p, list);
            }

            // fill out
            for (; ; )
            {
                for (int p = 1; p <= numberOfObjects; p++)
                {
                    // is this class still viable? 
                    if (lookup.ContainsKey(p))
                    {
                        List<int> indices = lookup[p];
                        List<int> nextSteps = new List<int>();

                        foreach (int currentIndex in indices)
                        {
                            int currentY = currentIndex / width;
                            int currentX = currentIndex % width;

                            //if(currentX == 39 && currentY == 89)
                            //{
                            //    return; 
                            //}

                            // identify possible next steps
                            for (int y = currentY - 1; y <= currentY + 1; y++)
                            {
                                for (int x = currentX - 1; x <= currentX + 1; x++)
                                {
                                    if (y >= 0 && y < height && x >= 0 && x < width)
                                    {
                                        int nextPossibleIndex = y * width + x;
                                        if (map[nextPossibleIndex] == 0)
                                        {
                                            nextSteps.Add(nextPossibleIndex);
                                        }
                                    }
                                }
                            }

                            if (nextSteps.Count > 0)
                            {
                                break;
                            }
                        }

                        if (nextSteps.Count > 0)
                        {
                            // random step
                            int nextStep = nextSteps[rand.Next(0, nextSteps.Count - 1)];
                            map[nextStep] = p;

                            lookup[p].Add(nextStep);
                        }
                        else
                        {
                            lookup.Remove(p);
                        }
                    }
                }

                if (lookup.Count == 0)
                {
                    break;
                }
            }

            Color[] colors = new Color[numberOfObjects];

            for (int c = 0; c < colors.Length; c++)
            {
                byte b = (byte)rand.Next(55, 89);
                colors[c] = Color.FromArgb(b, b, b);
            }

            using (Bitmap bmp = new Bitmap(width, height))
            {
                for (int y = 0, i = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++, i++)
                    {
                        int colorIndex = map[i];
                        if (colorIndex > 0)
                        {
                            bmp.SetPixel(x, y, colors[colorIndex - 1]);
                        }
                    }
                }

                GaussianBlur blur = new GaussianBlur(.5); 
                blur.ApplyInPlace(bmp); 

                bmp.Save("c:/users/brush/desktop/turd.bmp");
            }
        }
    }
}
