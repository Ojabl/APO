using System;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace APO
{
    class Lab2
    {
        public static BitmapSource InvertColors(BitmapSource source)
        {
            int width = source.PixelWidth;
            int height = source.PixelHeight;

            FormatConvertedBitmap grayscaleImage = new FormatConvertedBitmap();
            grayscaleImage.BeginInit();
            grayscaleImage.Source = source;
            grayscaleImage.DestinationFormat = PixelFormats.Gray8;
            grayscaleImage.EndInit();

            int bytesPerPixel = 1;
            int stride = width * bytesPerPixel;
            byte[] pixelData = new byte[height * stride];

            grayscaleImage.CopyPixels(pixelData, stride, 0);

            for (int i = 0; i < pixelData.Length; i++)
            {
                pixelData[i] = (byte)(255 - pixelData[i]);
            }

            return BitmapSource.Create(width, height, grayscaleImage.DpiX, grayscaleImage.DpiY, PixelFormats.Gray8, null, pixelData, stride);
        }

        public static Image<Bgr, byte> Equalization(Image<Bgr, byte> image, int minValue, int maxValue)
        {
            Image<Bgr, byte> result = image.Clone();
            Image<Gray, byte>[] channels = image.Split();

            for (int channel = 0; channel < 3; channel++)
            {
                var currentChannel = channels[channel];
                double[] minVal = new double[1];
                double[] maxVal = new double[1];
                System.Drawing.Point[] minLoc = new System.Drawing.Point[1];
                System.Drawing.Point[] maxLoc = new System.Drawing.Point[1];
                currentChannel.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);
                currentChannel._EqualizeHist();
                CvInvoke.Normalize(currentChannel, currentChannel, 0, 255, Emgu.CV.CvEnum.NormType.MinMax);

                CvInvoke.InsertChannel(currentChannel, result, channel);
            }
            return result;
        }
        public static Image<Bgr, byte> HistogramStretching(Image<Bgr, byte> inputImage, double minRange, double maxRange)
        {
            if (inputImage == null) return null;

            Image<Bgr, byte> outputImage = inputImage.Clone();
            double[] minVal, maxVal;
            Point[] minLoc, maxLoc;
            
            inputImage.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);

            double scaleFactor = (maxRange - minRange) / (maxVal[0] - minVal[0]);
            double offset = minRange - scaleFactor * minVal[0];

            for (int y = 0; y < inputImage.Rows; y++)
            {
                for (int x = 0; x < inputImage.Cols; x++)
                {
                    Bgr currentPixel = inputImage[y, x];
                    Bgr newPixel = new Bgr();
                    newPixel.Blue = Math.Min(Math.Max(currentPixel.Blue * scaleFactor + offset, minRange), maxRange);
                    newPixel.Green = Math.Min(Math.Max(currentPixel.Green * scaleFactor + offset, minRange), maxRange);
                    newPixel.Red = Math.Min(Math.Max(currentPixel.Red * scaleFactor + offset, minRange), maxRange);

                    outputImage[y, x] = newPixel;
                }
            }

            return outputImage;
        }

        public static Image<Bgr,byte> PosterizeImage(Image<Bgr,byte> inputImage, int graylevels)
        {
            int levelStep = 256 / graylevels;

            Image<Bgr, byte> posterizedImage = inputImage.Clone();

            for (int y = 0; y < posterizedImage.Height; y++)
            {
                for (int x = 0; x < posterizedImage.Width; x++)
                {
                    Bgr pixel = posterizedImage[y, x];
                    Bgr newPixel = new Bgr();

                    newPixel.Blue = (byte)(((int)(pixel.Blue / levelStep)) * levelStep + levelStep / 2);
                    newPixel.Green = (byte)(((int)(pixel.Green / levelStep)) * levelStep + levelStep / 2);
                    newPixel.Red = (byte)(((int)(pixel.Red / levelStep)) * levelStep + levelStep / 2);

                    posterizedImage[y, x] = newPixel;
                }
            }

            return posterizedImage;
        }
    }
}
