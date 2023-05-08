using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Win32;
using System.Windows;

namespace APO
{
    public partial class BlendTwoImagesWindow : Window
    {
        Image<Bgr, byte> firstImage = null;
        Image<Bgr, byte> secondImage = null;

        public BlendTwoImagesWindow()
        {
            InitializeComponent();
        }

        private void BtnFirstImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp";

            if(ofd.ShowDialog() == true ) firstImage = new Image<Bgr, byte>(ofd.FileName);
            else MessageBox.Show("Pick correct image!\nCorrect file extensions:\n.png\n.jpg\n.jpeg\n.bmp", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void BtnSecondImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp";

            if (ofd.ShowDialog() == true) secondImage = new Image<Bgr, byte>(ofd.FileName);
            else MessageBox.Show("Choose correct image!\nCorrect file extensions:\n.png\n.jpg\n.jpeg\n.bmp", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            if((firstImage != null && secondImage != null))
            {
                if(firstImage.Size == secondImage.Size)
                {
                    Image<Bgr,byte> outputImage = Lab3.BlendImages(firstImage, secondImage);

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Images must be the same size\nPlease pick another images", "Wrong size Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Choose both images!\nIt seems that one of them has not been chosen", "Both images not chosen Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}
