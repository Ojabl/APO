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
using Emgu.CV;
using Emgu.CV.Structure;
using System.Windows.Shapes;

namespace APO
{
    public partial class HistogramInputValues : Window
    {
        public HistogramInputValues()
        {
            InitializeComponent();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                int min = Int32.Parse(minValue.Text);
                int max = Int32.Parse(maxValue.Text);

                if(min < max && min >= 0 && min <= 255 & max >= 0 && max <= 255)
                {
                    Image<Bgr, byte> stretchedImage = Lab2.HistogramStretching(MainWindow.imgInput, min, max);
                    var newImageWindow = new OpenedImage
                    {
                        Title = "Stretched histogram - " + System.IO.Path.GetFileName(MainWindow.imageWindow.Title),
                        imageSquare = { Source = stretchedImage.ToBitmapSource() }
                    };
                    newImageWindow.Show();
                }
                else
                {
                    MessageBox.Show("Please input valid data\nmin and max value must be from range 0 to 255.\nmax must be greater than min\n", "Invalid data error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Please input valid data\nmin and max value must be from range 0 to 255.\nmax must be greater than min\n", "Invalid data error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
