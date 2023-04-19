using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Storage;
using System.Runtime.InteropServices;

namespace APO
{
    class Lab3
    {
        public static BitmapSource BlurImage(BitmapSource inputBitmap, int kernelSize)
        {
            // Copy the pixels of the input bitmap into a byte array
            int stride = (inputBitmap.PixelWidth * inputBitmap.Format.BitsPerPixel + 7) / 8;
            byte[] pixelData = new byte[stride * inputBitmap.PixelHeight];
            inputBitmap.CopyPixels(pixelData, stride, 0);

            // Create an Image<Bgr, byte> object from the input byte array
            Image<Bgr, byte> inputImage = new Image<Bgr, byte>(inputBitmap.PixelWidth, inputBitmap.PixelHeight);
            inputImage.Bytes = pixelData;

            // Create a "Box Blur" kernel
            double[,] kernelValues = new double[kernelSize, kernelSize];
            for (int i = 0; i < kernelSize; i++)
            {
                for (int j = 0; j < kernelSize; j++)
                {
                    kernelValues[i, j] = 1.0 / (kernelSize * kernelSize);
                }
            }
            System.Drawing.Size kSize = new System.Drawing.Size(kernelSize, kernelSize);
            
            // Apply the "Box Blur" kernel to the input image
            Image<Bgr, byte> outputImage = new Image<Bgr, byte>(inputImage.Size);
            System.Drawing.Point anchor = new System.Drawing.Point(-1, -1);
            CvInvoke.Blur(inputImage, outputImage,kSize,anchor);

            // Convert the output Image<Bgr, byte> to a BitmapSource
            byte[] outputData = outputImage.Bytes;
            int outputStride = outputImage.Width * outputImage.NumberOfChannels;
            BitmapSource outputBitmap = BitmapSource.Create(outputImage.Width, outputImage.Height, 96, 96, PixelFormats.Bgr24, null, outputData, outputStride);
            return outputBitmap;
        }

    }
}
