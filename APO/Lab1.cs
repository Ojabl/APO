using System;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Linq;
using System.Windows;
using ScottPlot;

namespace APO
{
    class Lab1
    {
        #region One-window converters
        public static Mat ConvertToGrayscale()
        {
            if (MainWindow.imageWindow != null)
            {
                Image<Gray, byte> imgOutput = new Image<Gray, byte>(MainWindow.imgInput.Width, MainWindow.imgInput.Height, new Gray(0));
                imgOutput = MainWindow.imgInput.Convert<Gray, byte>();

                MainWindow.imgInput = new Image<Bgr, byte>(imgOutput.Width, imgOutput.Height);
                CvInvoke.CvtColor(imgOutput, MainWindow.imgInput, Emgu.CV.CvEnum.ColorConversion.Gray2Bgr);
                
                Mat mat = imgOutput.Mat;
                mat.ToBitmap();

                return mat;
            }
            else return null;
        }
        public static Mat ConvertToHSV()
        {
            if (MainWindow.imageWindow != null)
            {
                Image<Hsv, byte> imageOutputHSV = new Image<Hsv, byte>(MainWindow.imgInput.Width, MainWindow.imgInput.Height);

                imageOutputHSV = MainWindow.imgInput.Convert<Hsv, byte>();

                Mat mat = imageOutputHSV.Mat;
                mat.ToBitmap();

                return mat;
            }
            else return null;
        }
        public static Mat ConvertToLab()
        {
            if (MainWindow.imageWindow != null)
            {
                Image<Lab, byte> imageOutputLAB = new Image<Lab, byte>(MainWindow.imgInput.Width, MainWindow.imgInput.Height);

                imageOutputLAB = MainWindow.imgInput.Convert<Lab, byte>();

                Mat mat = imageOutputLAB.Mat;
                mat.ToBitmap();

                return mat;
            }
            else return null;
        }
        #endregion

        #region three-window converters
        public static List<Mat> SplitHSVChannels()
        {
            if (MainWindow.imageWindow != null)
            {
                Image<Hsv, byte> imageOutputHSV = new Image<Hsv, byte>(MainWindow.imgInput.Width, MainWindow.imgInput.Height);
                imageOutputHSV = MainWindow.imgInput.Convert<Hsv, byte>();

                VectorOfMat vectorOfMats = new VectorOfMat();
                CvInvoke.Split(imageOutputHSV, vectorOfMats);

                List<Mat> mats = new List<Mat>();
                for (int i = 0; i < vectorOfMats.Size; i++) mats.Add(vectorOfMats[i]);

                return mats;
            }
            else return null;
        }
        public static List<Mat> SplitLABChannels()
        {
            if (MainWindow.imageWindow != null)
            {
                Image<Lab, byte> imageOutputLab = new Image<Lab, byte>(MainWindow.imgInput.Width, MainWindow.imgInput.Height);
                imageOutputLab = MainWindow.imgInput.Convert<Lab, byte>();

                VectorOfMat channels = new VectorOfMat();
                CvInvoke.Split(imageOutputLab, channels);

                List<Mat> mats = new List<Mat>();
                for (int i = 0; i < channels.Size; i++) mats.Add(channels[i]);
                
                return mats;
            }
            else return null;
        }
        #endregion

        #region plot profile compute and display intensity profile
        public static void DisplayIntensityProfile(double[] intensityProfile, Point startPoint, Point endPoint)
        {
            Histogram histogramWindow = new Histogram();
            histogramWindow.Title = "Plot Profile";
            var wpfPlot = new WpfPlot();

            double[] positions = Enumerable.Range(0, intensityProfile.Length).Select(i => (double)i).ToArray();
            wpfPlot.Plot.PlotScatter(positions, intensityProfile);
            wpfPlot.Plot.Title("Linia Profilu");
            wpfPlot.Plot.XLabel("Pozycja ");
            wpfPlot.Plot.YLabel("Intensywność");

            histogramWindow.histogramPlotContainer.Children.Add(wpfPlot);
            histogramWindow.Show();
            wpfPlot.Refresh();
        }
        public static double[] ComputeIntensityProfile(Image<Bgr, byte> image, Point startPoint, Point endPoint)
        {
            int numPoints = (int)Math.Ceiling(Math.Sqrt(Math.Pow(startPoint.X - endPoint.X, 2) + Math.Pow(startPoint.Y - endPoint.Y, 2)));
            double[] profile = new double[numPoints];

            for (int i = 0; i < numPoints; i++)
            {
                double t = (double)i / (numPoints - 1);
                int x = (int)Math.Round(startPoint.X * (1 - t) + endPoint.X * t);
                int y = (int)Math.Round(startPoint.Y * (1 - t) + endPoint.Y * t);
                Bgr pixel = image[x,y];
                byte gray = (byte)((pixel.Red * 0.299) + (pixel.Green * 0.587) + (pixel.Blue * 0.114));
                profile[i] = gray;
            }

            return profile;
        }
        #endregion
    }
}
