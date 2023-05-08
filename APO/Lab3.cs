using Emgu.CV.Structure;
using Emgu.CV;
using System;

namespace APO
{
    class Lab3
    {
        public static Image<Bgr, byte> GaussianBlur(Image<Bgr, byte> inputImage, int kernelSize, double sigma)
        {
            var outputImage = inputImage.SmoothGaussian(kernelSize, kernelSize, sigma, sigma);
            return outputImage;
        }

        public static Image<Gray, byte> SobelEdgeDetection(Image<Bgr, byte> inputImage)
        {
            var grayImage = inputImage.Convert<Gray, byte>();
            var sobelImage = grayImage.Sobel(0, 1, 3).Add(grayImage.Sobel(1, 0, 3)).AbsDiff(new Gray(0.0));
            var sobelImageByte = sobelImage.Convert<Gray, byte>();
            return sobelImageByte;
        }

        public static Image<Gray, byte> LaplacianEdgeDetection(Image<Bgr, byte> inputImage)
        {
            var grayImage = inputImage.Convert<Gray, byte>();
            var laplacianImage = grayImage.Laplace(3);
            var laplacianImageByte = laplacianImage.Convert<Gray, byte>();
            return laplacianImageByte;
        }

        public static Image<Gray, byte> CannyEdgeDetection(Image<Bgr, byte> inputImage, double threshold1, double threshold2)
        {
            var grayImage = inputImage.Convert<Gray, byte>();
            var cannyImage = grayImage.Canny(threshold1, threshold2);
            return cannyImage;
        }

        public static Image<Bgr, byte> ApplyLaplassianMask(Image<Bgr, byte> inputImage, int maskType)
        {
            int[,] laplacianMask;

            switch (maskType)
            {
                case 1:
                    laplacianMask = new int[,]
                    {
                        { 0, -1, 0 },
                        { -1, 5, -1 },
                        { 0, -1, 0 }
                    };
                    break;
                case 2:
                    laplacianMask = new int[,]
                    {
                        { -1, -1, -1 },
                        { -1, 9, -1 },
                        { -1, -1, -1 }
                    };
                    break;
                case 3:
                    laplacianMask = new int[,]
                    {
                        { 1, -2, 1 },
                        { -2, 5, -2 },
                        { 1, -2, 1 }
                    };
                    break;
                default:
                    throw new ArgumentException("Invalid mask type");
            }

            Matrix<float> floatMask = new Matrix<float>(3, 3);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    floatMask[i, j] = laplacianMask[i, j];
                }
            }

            Mat kernel = floatMask.Mat;

            var result = inputImage.CopyBlank();
            CvInvoke.Filter2D(inputImage, result, kernel, new System.Drawing.Point(-1, -1));
            
            MainWindow.imgInput = inputImage;
            
            return result;
        }

        internal static Image<Bgr,byte> ApplyCustomMask(Image<Bgr, byte> inputImage, int[,] customMask)
        {
            Matrix<float> floatMask = new Matrix<float>(3, 3);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    floatMask[i, j] = customMask[i, j];
                }
            }

            Mat kernel = floatMask.Mat;

            var result = inputImage.CopyBlank();
            CvInvoke.Filter2D(inputImage, result, kernel, new System.Drawing.Point(-1, -1));

            MainWindow.imgInput = inputImage;

            return result;
        }

        public static Image<Bgr,byte> MedianFilter(Image<Bgr,byte> input, int kernelSize)
        {
            Image<Bgr,byte> output = input.SmoothMedian(kernelSize);
            return output;
        }

        #region Mathematical operations

        public static Image<Bgr,byte> MathAdd(Image<Bgr, byte> input, int valueToAdd)
        {
            Image<Bgr,byte> output = input.CopyBlank();

            for(int y = 0; y < input.Height; y++)
            {
                for(int x = 0; x < input.Width; x++)
                {
                    Bgr pixel = input[x,y];
                    Bgr newPixel = new Bgr();

                    newPixel.Blue = (byte)(((int)(pixel.Blue + valueToAdd) > 255) ? 255 : (pixel.Blue + valueToAdd));
                    newPixel.Green = (byte)(((int)(pixel.Green + valueToAdd) > 255) ? 255 : (pixel.Green + valueToAdd));
                    newPixel.Red = (byte)(((int)(pixel.Red + valueToAdd) > 255) ? 255 : (pixel.Red + valueToAdd));

                    output[x,y] = newPixel;
                }
            }
            return output;
        }

        public static Image<Bgr, byte> MathSub(Image<Bgr, byte> input, int valueToSubtract)
        {
            Image<Bgr, byte> output = input.CopyBlank();

            for (int y = 0; y < input.Height; y++)
            {
                for (int x = 0; x < input.Width; x++)
                {
                    Bgr pixel = input[x, y];
                    Bgr newPixel = new Bgr();

                    newPixel.Blue = (byte)(((int)(pixel.Blue - valueToSubtract) < 0) ? 0 : (pixel.Blue + valueToSubtract));
                    newPixel.Green = (byte)(((int)(pixel.Green - valueToSubtract) < 0) ? 0 : (pixel.Green + valueToSubtract));
                    newPixel.Red = (byte)(((int)(pixel.Red - valueToSubtract) < 0) ? 0 : (pixel.Red + valueToSubtract));

                    output[x, y] = newPixel;
                }
            }
            return output;
        } // TODO: check if it works

        public static Image<Bgr,byte> BlendImages(Image<Bgr,byte> firstImage, Image<Bgr, byte> secondImage)
        {
            float blendAlpha = 0.5f;
            
            Image<Bgr, byte> outputImage = new Image<Bgr, byte>(firstImage.Size);
            
            CvInvoke.AddWeighted(firstImage,blendAlpha,secondImage, 1-blendAlpha,0, outputImage);

            return outputImage;
        }


        #endregion 
    }
}