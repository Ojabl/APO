using Emgu.CV;
using Emgu.CV.Structure;
using ScottPlot.Plottable;
using System.Windows;

namespace APO
{
    public partial class TwoStageFilteringWindow : Window
    {
        private float[,] sharpeningMask = new float[3, 3];
        private float[,] smoothingMask = new float[3, 3];
        private int sharpeningMaskType;
        private int smoothingMaskType;

        public TwoStageFilteringWindow()
        {
            InitializeComponent();
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            if(RbtnHighSharpening.IsChecked == true) sharpeningMaskType = 1;
            else sharpeningMaskType = 2;

            if (RbtnHighSmoothing.IsChecked == true) smoothingMaskType = 1;
            else smoothingMaskType = 2;

            HandleMaskCreation();

            //handle filtering
            Image<Gray,byte> filteredImage = Lab4.TwoStepFiltering(MainWindow.imgInput.Convert<Gray,byte>(), smoothingMask, sharpeningMask);

            OpenedImage filteredImageWindow = new OpenedImage()
            {
                imageSquare = { Source = filteredImage.ToBitmapSource() },
                Title = "Filtered image"
            };
            filteredImageWindow.Show();

            this.Close();
        }

        private void HandleMaskCreation()
        {
            switch(smoothingMaskType, sharpeningMaskType)
            {
                case (1, 1):
                    this.sharpeningMask = new float[,]
                    {
                        { -1, -1, -1 },
                        { -1, 9, -1 },
                        { -1, -1, -1 }
                    };
                    this.smoothingMask = new float[,]
                    {
                        { 1f/9, 1f/9, 1f/9 },
                        { 1f/9, 1f/9, 1f/9 },
                        { 1f/9, 1f/9, 1f/9 }
                    };
                    break;
                case (1, 2):
                    this.sharpeningMask = new float[,]
                    {
                        { -1, -1, -1 },
                        { -1, 9, -1 },
                        { -1, -1, -1 }
                    };
                    this.smoothingMask = new float[,]
                    {
                        { 1f/16, 1f/8, 1f/16 },
                        { 1f/8, 1f/4, 1f/8 },
                        { 1f/16, 1f/8, 1f/16 }
                    };
                    break;
                case (2, 1):
                    this.sharpeningMask = new float[,]
                    {
                        { 0, -1, 0 },
                        { -1, 5, -1 },
                        { 0, -1, 0 }
                    };
                    this.smoothingMask = new float[,]
                    {
                        { 1f/16, 1f/8, 1f/16 },
                        { 1f/8, 1f/4, 1f/8 },
                        { 1f/16, 1f/8, 1f/16 }
                    };
                    break;
                case (2, 2):
                    this.sharpeningMask = new float[,]
                    {
                        { 0, -1, 0 },
                        { -1, 5, -1 },
                        { 0, -1, 0 }
                    };
                    this.smoothingMask = new float[,]
                    {
                        { 1f/16, 1f/8, 1f/16 },
                        { 1f/8, 1f/4, 1f/8 },
                        { 1f/16, 1f/8, 1f/16 }
                    };
                    break;
            }
        }
    }
}
