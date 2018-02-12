using MRCSharpLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Tester
{
    class Program
    {
        private static void SavePLY(string outputFile, List<PLYVertex> vertices)
        {
            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                sw.WriteLine("ply");
                sw.WriteLine("format ascii 1.0");
                sw.WriteLine("element vertex " + vertices.Count.ToString());
                sw.WriteLine("property float x");
                sw.WriteLine("property float y");
                sw.WriteLine("property float z");
                sw.WriteLine("property uchar red");
                sw.WriteLine("property uchar green");
                sw.WriteLine("property uchar blue");
                sw.WriteLine("element face " + 0.ToString());
                sw.WriteLine("property list uchar uint vertex_indices");
                sw.WriteLine("end_header");

                foreach (PLYVertex vertex in vertices)
                {
                    sw.WriteLine("{0} {1} {2} {3} {4} {5}", vertex.X, vertex.Y, vertex.Z,
                        Color.White.R, Color.White.G, Color.White.B);
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Loading main file.."); 
            MRCFile file = MRCParser.Parse(@"C:\Users\Ben\desktop\tomograms\tomography2_fullsirtcliptrim.mrc");

            Console.WriteLine("Loading label files..."); 
            List<MRCFile> labelFiles = new List<MRCFile>();
            labelFiles.Add(MRCParser.Parse(@"C:\Users\Ben\desktop\tomograms\tomography2_fullsirtcliptrim.labels.mrc"));
            labelFiles.Add(MRCParser.Parse(@"C:\Users\Ben\desktop\tomograms\tomography2_fullsirtcliptrim.labels2.mrc"));
            labelFiles.Add(MRCParser.Parse(@"C:\Users\Ben\desktop\tomograms\tomography2_fullsirtcliptrim.labels3.mrc"));
            labelFiles.Add(MRCParser.Parse(@"C:\Users\Ben\desktop\tomograms\tomography2_fullsirtcliptrim.labels4.mrc"));
            labelFiles.Add(MRCParser.Parse(@"C:\Users\Ben\desktop\tomograms\tomography2_fullsirtcliptrim.labels5.mrc"));
            labelFiles.Add(MRCParser.Parse(@"C:\Users\Ben\desktop\tomograms\tomography2_fullsirtcliptrim.labels7.mrc"));

            Color[] colors = new Color[]
            {
                Color.Red, Color.Green, Color.Pink, Color.Orange, Color.Yellow, Color.Violet
            };

            float scaler = 255 / (file.MaxPixelValue - file.MinPixelValue);

            List<PLYVertex> vertices = new List<PLYVertex>();
            for (int z = 0; z < file.Frames.Count; z++)
            {
                Console.WriteLine($"File {z}/{file.Frames.Count}");
                MRCFrame frame = file.Frames[z];

                using (Bitmap bmp = new Bitmap(frame.Width, frame.Height))
                {
                    for (int y = 0, i = 0; y < frame.Height; y++)
                    {
                        for (int x = 0; x < frame.Width; x++, i++)
                        {
                            bool labeled = false;
                            int colorIndex = 0;
                            foreach (MRCFile label in labelFiles)
                            {
                                if (label.Frames[z].Data[i] > 0)
                                {
                                    byte b = (byte)(frame.Data[i] * scaler);
                                    //bmp.SetPixel(x, y, colors[colorIndex]);
                                    vertices.Add(new PLYVertex { X = x, Y = y, Z = z, Color = colors[colorIndex] });
                                    labeled = true;
                                }

                                colorIndex++;
                            }

                            if (!labeled)
                            {
                                byte b = (byte)(frame.Data[i] * scaler);
                                //bmp.SetPixel(x, y, Color.FromArgb(b, b, b));

                                vertices.Add(new PLYVertex { X = x, Y = y, Z = z, Color = Color.FromArgb(b, b, b) });
                            }
                        }
                    }

                    //bmp.Save($"c:/users/ben/desktop/images/{z}.png", ImageFormat.Png);
                }
            }

            Console.WriteLine("Saving...");
            SavePLY("C:/users/ben/desktop/pointCloud.ply", vertices);
        }
    }
}
