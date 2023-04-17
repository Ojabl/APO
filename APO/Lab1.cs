using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using Emgu;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.ImgHash;
using Emgu.CV.Util;

namespace APO
{
    class Lab1
    {
        public static Mat ConvertToGrayscale()
        {
            if (MainWindow.imageWindow != null)
            {
                Image<Gray, byte> imgOutput = new Image<Gray, byte>(MainWindow.imgInput.Width, MainWindow.imgInput.Height, new Gray(0));
                imgOutput = MainWindow.imgInput.Convert<Gray, byte>();

                Mat mat = imgOutput.Mat;
                mat.ToBitmap();

                return mat;
            }
            else
            {
                return null;
            }
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
            else
            {
                return null;
            }
        }
        public static Mat ConvertToLab()
        {
            if (MainWindow.imageWindow != null)
            {
                Image<Lab, byte> imageOutputHSV = new Image<Lab, byte>(MainWindow.imgInput.Width, MainWindow.imgInput.Height);

                imageOutputHSV = MainWindow.imgInput.Convert<Lab, byte>();

                Mat mat = imageOutputHSV.Mat;
                mat.ToBitmap();

                return mat;
            }
            else
            {
                return null;
            }
        }

        public static List<Mat> SplitHSVChannels()
        {
            if (MainWindow.imageWindow != null)
            {
                Image<Hsv, byte> imageOutputHSV = new Image<Hsv, byte>(MainWindow.imgInput.Width, MainWindow.imgInput.Height);
                imageOutputHSV = MainWindow.imgInput.Convert<Hsv, byte>();

                VectorOfMat vectorOfMats = new VectorOfMat();
                CvInvoke.Split(imageOutputHSV, vectorOfMats);

                List<Mat> mats = new List<Mat>();
                for (int i = 0; i < vectorOfMats.Size; i++)
                {
                    mats.Add(vectorOfMats[i]);
                }

                return mats;
            }
            else
            {
                return null;
            }
        }

        public static List<Mat> ConvertToLabThreeChannels()
        {
            if (MainWindow.imageWindow != null)
            {
                Image<Lab, byte> imageOutputLab = new Image<Lab, byte>(MainWindow.imgInput.Width, MainWindow.imgInput.Height);
                imageOutputLab = MainWindow.imgInput.Convert<Lab, byte>();

                // Split the Lab image into its channels
                VectorOfMat channels = new VectorOfMat();
                CvInvoke.Split(imageOutputLab, channels);

                // Convert the VectorOfMat to a list of Mat
                List<Mat> mats = new List<Mat>();
                for (int i = 0; i < channels.Size; i++)
                {
                    mats.Add(channels[i]);
                }

                return mats;
            }
            else
            {
                return null;
            }
        }
    }
}
