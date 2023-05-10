using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using static APO.OpenedImage;

namespace APO
{
    class Lab4
    {
        #region Morphological operations

        public static Mat structurizingElementMat = new Mat();

        public static Image<Bgr,byte> Binarization(Image<Bgr, byte> input)
        {
            Image<Gray, byte> output = ToGrayImage(input);

            Gray thresholdValue = new Gray(128);
            Gray maxValue = new Gray(255);

            CvInvoke.Threshold(output, output, thresholdValue.Intensity, maxValue.Intensity, Emgu.CV.CvEnum.ThresholdType.Binary);

            MainWindow.imgInput = output.Convert<Bgr, byte>();
            binarizedImage = output;

            return output.Convert<Bgr, byte>();
        }

        public static Image<Bgr,byte> Erosion(Image<Bgr, byte> input, StructurizingElement structurizingElement)
        {
            Image<Bgr, byte> output = new Image<Bgr, byte>(input.Size);

            BuildElement(structurizingElement);

            CvInvoke.Erode(input, output, structurizingElementMat, new System.Drawing.Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0));
            
            MainWindow.imgInput = output;
            binarizedImage = output.Convert<Gray, byte>();
            return output;
        }

        public static Image<Bgr, byte> Dilation(Image<Bgr, byte> input, StructurizingElement structurizingElement)
        {
            Image<Bgr, byte> output = new Image<Bgr, byte>(input.Size);

            BuildElement(structurizingElement);

            CvInvoke.Dilate(input, output, structurizingElementMat, new System.Drawing.Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Default, new MCvScalar(0));

            MainWindow.imgInput = output;
            return output;
        }

        public static Image<Bgr, byte> Open(Image<Bgr, byte> input, StructurizingElement structurizingElement)
        {
            Image<Bgr, byte> output = new Image<Bgr, byte>(input.Size);

            BuildElement(structurizingElement);



            MainWindow.imgInput = output;
            return output;
        }

        public static Image<Bgr, byte> Close(Image<Bgr, byte> input, StructurizingElement structurizingElement)
        {
            Image<Bgr, byte> output = new Image<Bgr, byte>(input.Size);

            BuildElement(structurizingElement);



            MainWindow.imgInput = output;
            return output;
        }

        public static Image<Bgr, byte> Skeletize(Image<Bgr, byte> input, StructurizingElement structurizingElement)
        {
            Image<Bgr, byte> output = new Image<Bgr, byte>(input.Size);

            BuildElement(structurizingElement);


            return output;
        }

        private static void BuildElement(StructurizingElement structurizingElement)
        {
            switch (structurizingElement)
            {
                case StructurizingElement.Diamond:
                    structurizingElementMat = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Cross, new System.Drawing.Size(3, 3), new System.Drawing.Point(-1, -1));
                    break;
                case StructurizingElement.Square:
                    structurizingElementMat = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new System.Drawing.Size(3, 3), new System.Drawing.Point(-1, -1));
                    break;
            }
        }

        private static Image<Gray,byte> ToGrayImage(Image<Bgr,byte> input)
        {
            return input.Convert<Gray, byte>();
        }

        #endregion
    }
}
