using MRCSharpLib;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            MRCFile file = MRCParser.Parse(@"C:\Users\Ben\Downloads\tomography2_fullsirtcliptrim.mrc");
            MRCFile label4File = MRCParser.Parse(@"C:\Users\Ben\Downloads\tomography2_fullsirtcliptrim.labels4.mrc");
            MRCFile label7File = MRCParser.Parse(@"C:\Users\Ben\Downloads\tomography2_fullsirtcliptrim.labels7.mrc");

            float scaler = 255 / (file.MaxPixelValue - file.MinPixelValue);

            for (int c = 0; c < file.Frames.Count; c++)
            {
                MRCFrame frame = file.Frames[c];

                using (Bitmap bmp = new Bitmap(frame.Width, frame.Height))
                {
                    for (int y = 0, i = 0; y < frame.Height; y++)
                    {
                        for (int x = 0; x < frame.Width; x++, i++)
                        {
                            if(label4File.Frames[c].Data[i] > 0)
                            {
                                byte b = (byte)(frame.Data[i] * scaler);
                                bmp.SetPixel(x, y, Color.Red);
                            }
                            else if (label7File.Frames[c].Data[i] > 0)
                            {
                                byte b = (byte)(frame.Data[i] * scaler);
                                bmp.SetPixel(x, y, Color.Green);
                            }
                            else
                            {
                                byte b = (byte)(frame.Data[i] * scaler);
                                bmp.SetPixel(x, y, Color.FromArgb(b, b, b));
                            }
                        }
                    }

                    bmp.Save($"c:/users/ben/desktop/images/{c}.png", ImageFormat.Png); 
                }
            }
        }
    }
}
