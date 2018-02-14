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
            MRCFile file = MRCParser.Parse(@"E:\Downloads\tomograms\tomography2_fullsirtcliptrim.mrc");

            Console.WriteLine("Loading label files...");
            List<MRCFile> labelFiles = new List<MRCFile>();
            labelFiles.Add(MRCParser.Parse(@"E:\Downloads\tomograms\tomography2_fullsirtcliptrim.labels.mrc"));
            labelFiles.Add(MRCParser.Parse(@"E:\Downloads\tomograms\tomography2_fullsirtcliptrim.labels2.mrc"));
            labelFiles.Add(MRCParser.Parse(@"E:\Downloads\tomograms\tomography2_fullsirtcliptrim.labels3.mrc"));
            labelFiles.Add(MRCParser.Parse(@"E:\Downloads\tomograms\tomography2_fullsirtcliptrim.labels4.mrc"));
            labelFiles.Add(MRCParser.Parse(@"E:\Downloads\tomograms\tomography2_fullsirtcliptrim.labels5.mrc"));
            labelFiles.Add(MRCParser.Parse(@"E:\Downloads\tomograms\tomography2_fullsirtcliptrim.labels7.mrc"));

            Color[] colors = new Color[]
            {
                Color.Red, Color.Green, Color.Pink, Color.Orange, Color.Yellow, Color.Violet
            };

            float scaler = 255 / (file.MaxPixelValue - file.MinPixelValue);

            int vertexCount = 0;
            using (StreamWriter sw = new StreamWriter("C:/users/ben/desktop/pointCloudEnding.ply"))
            {
                for (int z = 0; z < file.Frames.Count; z++)
                {
                    Console.WriteLine($"File {z}/{file.Frames.Count}");
                    MRCFrame frame = file.Frames[z];

                    for (int y = 0, i = 0; y < frame.Height; y++)
                    {
                        for (int x = 0; x < frame.Width; x++, i++)
                        {
                            try
                            {
                                bool labeled = false;
                                int colorIndex = 0;
                                foreach (MRCFile label in labelFiles)
                                {
                                    if (label.Frames[z].Data[i] > 0)
                                    {
                                        byte b = (byte)(frame.Data[i] * scaler);
                                        //bmp.SetPixel(x, y, colors[colorIndex]);
                                        for (int y1 = 0; y1 < 1; y1++)
                                        {
                                            for (int x1 = 0; x1 < 1; x1++)
                                            {
                                                for (int z1 = 0; z1 < 1; z1++)
                                                {
                                                    sw.WriteLine("{0} {1} {2} {3} {4} {5}", 
                                                        x + x1 * file.PixelSize, 
                                                        y + y1 * file.PixelSize, 
                                                        z + z1 * file.PixelSize,
                                                        colors[colorIndex].R, colors[colorIndex].G, colors[colorIndex].B);
                                                }
                                            }
                                        }

                                        //vertices.Add(new PLYVertex { X = x, Y = y, Z = z, Color = colors[colorIndex] });
                                        labeled = true;
                                        vertexCount++;
                                    }

                                    colorIndex++;
                                }

                                //if (!labeled)
                                //{
                                //    byte b = (byte)(frame.Data[i] * scaler);
                                //    //bmp.SetPixel(x, y, Color.FromArgb(b, b, b));
                                //    sw.WriteLine("{0} {1} {2} {3} {4} {5}", x, y, z, b, b, b);
                                //    vertexCount++;
                                //    //vertices.Add(new PLYVertex { X = x, Y = y, Z = z, Color = Color.FromArgb(b, b, b) });
                                //}
                            }
                            catch
                            {
                                //sw.WriteLine("{0} {1} {2} {3} {4} {5}", x, y, z, 0, 0, 0);
                                //vertexCount++;
                                //vertices.Add(new PLYVertex { X = x, Y = y, Z = z, Color = Color.FromArgb(0, 0, 0) });
                            }
                        }
                    }

                    //bmp.Save($"c:/users/ben/desktop/images/{z}.png", ImageFormat.Png);
                }
            }

            using (StreamWriter sw = new StreamWriter("c:/users/ben/desktop/pointCloud.ply"))
            {
                sw.WriteLine("ply");
                sw.WriteLine("format ascii 1.0");
                sw.WriteLine("element vertex " + vertexCount.ToString());
                sw.WriteLine("property float x");
                sw.WriteLine("property float y");
                sw.WriteLine("property float z");
                sw.WriteLine("property uchar red");
                sw.WriteLine("property uchar green");
                sw.WriteLine("property uchar blue");
                sw.WriteLine("element face " + 0.ToString());
                sw.WriteLine("property list uchar uint vertex_indices");
                sw.WriteLine("end_header");

                using (StreamReader sr = new StreamReader("C:/users/ben/desktop/pointCloudEnding.ply"))
                {
                    string l = null;
                    while ((l = sr.ReadLine()) != null)
                    {
                        sw.WriteLine(l);
                    }
                }
            }
        }
    }
}
