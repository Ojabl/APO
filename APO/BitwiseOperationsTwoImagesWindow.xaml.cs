using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Win32;
using System.Windows;

namespace APO
{
    public partial class OperationTypesTwoImagesWindow : Window
    {
        Image<Bgr, byte>? firstImage = null;
        Image<Bgr, byte>? secondImage = null;
        MainWindow.OperationType operation;

        OpenFileDialog ofd = new()
        {
            Filter = "Image Files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp"
        };

        public OperationTypesTwoImagesWindow()
        {
            InitializeComponent();
        }

        private void BtnFirstImage_Click(object sender, RoutedEventArgs e)
        {
            if (ofd.ShowDialog() == true) firstImage = new Image<Bgr, byte>(ofd.FileName);
            else MessageBox.Show("Pick correct image!\nCorrect file extensions:\n.png\n.jpg\n.jpeg\n.bmp", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            if (firstImage != null)
            {
                ImgLeft.Source = firstImage.ToBitmapSource();
            }
        }

        private void BtnSecondImage_Click(object sender, RoutedEventArgs e)
        {
            if (ofd.ShowDialog() == true) secondImage = new Image<Bgr, byte>(ofd.FileName);
            else MessageBox.Show("Choose correct image!\nCorrect file extensions:\n.png\n.jpg\n.jpeg\n.bmp", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            if (secondImage != null)
            {
                ImgRight.Source = secondImage.ToBitmapSource();
            }
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            if ((firstImage != null && secondImage != null))
            {
                if (firstImage.Size == secondImage.Size)
                {
                    Image<Bgr, byte> resultImage = new(firstImage.Size);
                    operation = MainWindow.operationType;

                    switch (operation)
                    {
                        case MainWindow.OperationType.Blend:
                            resultImage = Lab3.BlendImages(firstImage, secondImage);
                            break;

                        case MainWindow.OperationType.AND:
                            CvInvoke.BitwiseAnd(firstImage, secondImage, resultImage);
                            break;

                        case MainWindow.OperationType.OR:
                            CvInvoke.BitwiseOr(firstImage, secondImage, resultImage);
                            break;

                        case MainWindow.OperationType.XOR:
                            CvInvoke.BitwiseXor(firstImage, secondImage, resultImage);
                            break;
                    }

                    OpenedImage imageAfterOperation = new OpenedImage()
                    {
                        imageSquare = { Source = resultImage.ToBitmapSource() },
                        Title = "Image after " + operation.ToString() + " operation",
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };
                    imageAfterOperation.Show();

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Images must be the same size\nPlease pick other images", "Wrong size Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Choose both images!\nIt seems that one of them has not been chosen", "Both images not chosen Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
