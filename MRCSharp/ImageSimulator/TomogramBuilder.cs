using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSimulator
{
    public static class TomogramBuilder
    {
        public static void SaveAsBitmap(Tomogram tomogram, string path)
        {
            Color[] colors = new Color[tomogram.BackgroundDensity];

            for (int c = 0; c < colors.Length; c++)
            {
                byte b = (byte)tomogram.Random.Next(55, 89);
                colors[c] = Color.FromArgb(b, b, b);
            }

            using (Bitmap bmp = new Bitmap(tomogram.Width, tomogram.Height))
            {
                for (int y = 0, i = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++, i++)
                    {
                        int colorIndex = tomogram.Data[i];
                        if (colorIndex > 0)
                        {
                            bmp.SetPixel(x, y, colors[colorIndex - 1]);
                        }
                    }
                }

                GaussianBlur blur = new GaussianBlur(1, 10);
                blur.ApplyInPlace(bmp);

                bmp.Save(path);
            }
        }

        public static Tomogram BuildTomogram(int width, int height, 
            int backgroundDensity, int vesicleCount)
        {
            Tomogram ret = new Tomogram();
            ret.Width = width;
            ret.Height = height;
            ret.BackgroundDensity = backgroundDensity;
            ret.VesicleCount = vesicleCount;
            ret.Random = new Random(); 

            BuildBackground(ret); 

            return ret; 
        }

        private static void BuildBackground(Tomogram tom)
        {
            tom.Data = new int[tom.Width * tom.Height];

            Dictionary<int, List<int>> lookup = new Dictionary<int, List<int>>();

            // initialize
            for (int p = 1; p <= tom.BackgroundDensity; p++)
            {
                int x = tom.Random.Next(0, tom.Width);
                int y = tom.Random.Next(0, tom.Height);
                int index = y * tom.Width + x;

                tom.Data[index] = p;
                List<int> list = new List<int>();
                list.Add(index);
                lookup.Add(p, list);
            }

            // fill out
            for (; ; )
            {
                for (int p = 1; p <= tom.BackgroundDensity; p++)
                {
                    // is this class still viable? 
                    if (lookup.ContainsKey(p))
                    {
                        List<int> indices = lookup[p];
                        List<int> nextSteps = new List<int>();

                        foreach (int currentIndex in indices)
                        {
                            int currentY = currentIndex / tom.Width;
                            int currentX = currentIndex % tom.Width;

                            // identify possible next steps
                            for (int y = currentY - 1; y <= currentY + 1; y++)
                            {
                                for (int x = currentX - 1; x <= currentX + 1; x++)
                                {
                                    if (y >= 0 && y < tom.Height && x >= 0 && x < tom.Width)
                                    {
                                        int nextPossibleIndex = y * tom.Width + x;
                                        if (tom.Data[nextPossibleIndex] == 0)
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
                            int nextStep = nextSteps[tom.Random.Next(0, nextSteps.Count - 1)];
                            tom.Data[nextStep] = p;

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
        }
    }
}
