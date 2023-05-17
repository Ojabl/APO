using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using static APO.OpenedImage;

namespace APO
{
    class Lab4
    {
        #region Morphological operations

        public static Mat structurizingElementMat = new Mat();
        public static ElementShape shape = new ElementShape();

        public static Image<Bgr,byte> Binarization(Image<Bgr, byte> input)
        {
            Image<Gray, byte> output = ToGrayImage(input);

            Gray thresholdValue = new Gray(128);
            Gray maxValue = new Gray(255);

            CvInvoke.Threshold(output, output, thresholdValue.Intensity, maxValue.Intensity, ThresholdType.Binary);

            MainWindow.imgInput = output.Convert<Bgr, byte>();

            return output.Convert<Bgr, byte>();
        }

        public static Image<Bgr,byte> Erosion(Image<Bgr, byte> input, StructurizingElement structurizingElement)
        {
            Image<Bgr, byte> output = new Image<Bgr, byte>(input.Size);

            BuildElement(structurizingElement);

            CvInvoke.Erode(input, output, structurizingElementMat, new System.Drawing.Point(-1, -1), 1, BorderType.Default, new MCvScalar(0));
            
            MainWindow.imgInput = output;
            return output;
        }

        public static Image<Bgr, byte> Dilation(Image<Bgr, byte> input, StructurizingElement structurizingElement)
        {
            Image<Bgr, byte> output = new Image<Bgr, byte>(input.Size);

            BuildElement(structurizingElement);

            CvInvoke.Dilate(input, output, structurizingElementMat, new System.Drawing.Point(-1, -1), 1, BorderType.Default, new MCvScalar(0));

            MainWindow.imgInput = output;
            return output;
        }

        public static Image<Bgr, byte> Open(Image<Bgr, byte> input, StructurizingElement structurizingElement)
        {
            Image<Bgr, byte> erodedImage = Erosion(input, structurizingElement);
            Image<Bgr, byte> openedImage = Dilation(erodedImage, structurizingElement);

            MainWindow.imgInput = openedImage;
            return openedImage;
        }

        public static Image<Bgr, byte> Close(Image<Bgr, byte> input, StructurizingElement structurizingElement)
        {
            Image<Bgr, byte> openedImage = Dilation(input, structurizingElement);
            Image<Bgr, byte> erodedImage = Erosion(openedImage, structurizingElement);

            MainWindow.imgInput = erodedImage;
            return erodedImage;
        }

        public static Image<Gray, byte> Skeletonize(Image<Bgr, byte> input, StructurizingElement structurizingElement)
        {
            Image<Gray, byte> imgOld = input.Convert<Gray,byte>();
            Image<Gray, byte> img2 = (new Image<Gray, byte>(imgOld.Width, imgOld.Height, new Gray(255))).Sub(imgOld);
            Image<Gray, byte> eroded = new Image<Gray, byte>(img2.Size);
            Image<Gray, byte> temp = new Image<Gray, byte>(img2.Size);
            Image<Gray, byte> skel = new Image<Gray, byte>(img2.Size);

            skel.SetValue(0);
            CvInvoke.Threshold(img2, img2, 127, 256, 0);
            var element = CvInvoke.GetStructuringElement(ElementShape.Cross, new Size(3, 3), new Point(-1, -1));
            bool done = false;

            while (!done)
            {
                CvInvoke.Erode(img2, eroded, element, new Point(-1, -1), 1, BorderType.Reflect, default(MCvScalar));
                CvInvoke.Dilate(eroded, temp, element, new Point(-1, -1), 1, BorderType.Reflect, default(MCvScalar));
                CvInvoke.Subtract(img2, temp, temp);
                CvInvoke.BitwiseOr(skel, temp, skel);
                eroded.CopyTo(img2);
                if (CvInvoke.CountNonZero(img2) == 0) done = true;
            }
            return skel;
        }

        #region helping methods

        private static void BuildElement(StructurizingElement structurizingElement)
        {
            switch (structurizingElement)
            {
                case StructurizingElement.Diamond:
                    structurizingElementMat = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new System.Drawing.Size(3, 3), new System.Drawing.Point(-1, -1));
                    shape = ElementShape.Cross;
                    break;
                case StructurizingElement.Square:
                    structurizingElementMat = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new System.Drawing.Size(3, 3), new System.Drawing.Point(-1, -1));
                    shape = ElementShape.Rectangle;
                    break;
            }
        }
        private static Image<Gray,byte> ToGrayImage(Image<Bgr,byte> input)
        {
            return input.Convert<Gray, byte>();
        }

        #endregion

        #endregion

        #region Two step filtering

        public static Image<Gray, byte> TwoStepFiltering(Image<Gray, byte> inputImage, float[,] smoothingMask, float[,] sharpeningMask)
        {
            // Apply smoothing
            ConvolutionKernelF smoothingKernel = new ConvolutionKernelF(smoothingMask);
            Image<Gray, float> smoothedImg = inputImage.Convert<Gray, float>().Convolution(smoothingKernel);

            // Apply sharpening
            ConvolutionKernelF sharpeningKernel = new ConvolutionKernelF(sharpeningMask);
            Image<Gray, float> sharpenedImg = smoothedImg.Convolution(sharpeningKernel);

            // Calculate 5x5 mask
            float[,] mask5x5 = new float[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    mask5x5[i, j] = smoothingMask[i / 2, j / 2] * sharpeningMask[i / 2, j / 2];
                }
            }

            // Apply 5x5 mask
            ConvolutionKernelF kernel5x5 = new ConvolutionKernelF(mask5x5);
            Image<Gray, float> finalImg = inputImage.Convert<Gray, float>().Convolution(kernel5x5);

            // Compare results
            double diff = CvInvoke.Norm(sharpenedImg, finalImg, NormType.L2);
            Console.WriteLine($"Difference between two-step filtering and 5x5 mask: {diff}");

            return finalImg.Convert<Gray, byte>();
        }

        #endregion
    }
}
