using Emgu.CV.Structure;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APO_Projekt_1
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
    }
}