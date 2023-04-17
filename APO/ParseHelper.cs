using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Win32;
using ScottPlot;
using ScottPlot.WPF;
namespace APO
{
    class ParseHelper
    {
        public static Image<Bgr, byte> ConvertImageSourceToEmguImage(ImageSource source)
        {
            BitmapSource bitmapSource = (BitmapSource)source;
            int width = bitmapSource.PixelWidth;
            int height = bitmapSource.PixelHeight;
            int stride = width * 4; // Assuming 32 bits per pixel (ARGB)
            byte[] pixelData = new byte[stride * height];

            bitmapSource.CopyPixels(pixelData, stride, 0);

            Image<Bgr, byte> image = new Image<Bgr, byte>(width, height);

            for (int y = 0; y < height; y++)
            {
                int baseIndex = y * stride;
                for (int x = 0; x < width; x++)
                {
                    int index = baseIndex + x * 4;
                    image[y, x] = new Bgr(pixelData[index + 2], pixelData[index + 1], pixelData[index]);
                }
            }
            return image;
        }
    }
}
