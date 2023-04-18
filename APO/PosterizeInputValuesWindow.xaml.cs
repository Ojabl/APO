using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Emgu.CV;
using Emgu.CV.Structure;

namespace APO
{
    public partial class PosterizeInputValuesWindow : Window
    {
        public PosterizeInputValuesWindow()
        {
            InitializeComponent();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int grayLevels = int.Parse(TbGrayLevels.Text.ToString());

                if(grayLevels >= 0 && grayLevels <= 256 && grayLevels != 0)
                {
                    Image<Bgr, byte> posterizedImage = Lab2.PosterizeImage(MainWindow.imgInput, grayLevels);

                    var posterizedImageWindow = new OpenedImage
                    {
                        Title = "Posterized image - " + grayLevels + " gray levels - " + System.IO.Path.GetFileName(MainWindow.imageWindow.Title),
                        imageSquare = { Source = posterizedImage.ToBitmapSource() }
                    };
                    posterizedImageWindow.Show();
                    posterizedImageWindow.image = new Image();
                }
                else MessageBox.Show("Please input valid data\nvalue must be from range 0 to 255.", "Invalid data error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {
                MessageBox.Show("Please input valid data\nvalue must be from range 0 to 255.", "Invalid data error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
