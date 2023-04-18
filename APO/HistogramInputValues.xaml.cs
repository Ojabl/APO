using System;
using System.Windows;
using Emgu.CV;
using Emgu.CV.Structure;

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
                int min = int.Parse(minValue.Text);
                int max = int.Parse(maxValue.Text);

                if(min < max && min >= 0 && min <= 255 & max >= 0 && max <= 255)
                {
                    Image<Bgr, byte> stretchedImage = Lab2.HistogramStretching(MainWindow.imgInput, min, max);
                    var stretchedImageWindow = new OpenedImage
                    {
                        Title = "Stretched histogram - " + System.IO.Path.GetFileName(MainWindow.imageWindow.Title),
                        imageSquare = { Source = stretchedImage.ToBitmapSource() }
                    };
                    stretchedImageWindow.Show();
                    this.Close();
                }
                else MessageBox.Show("Please input valid data\nmin and max value must be from range 0 to 255.\nmax must be greater than min\n", "Invalid data error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {
                MessageBox.Show("Please input valid data\nmin and max value must be from range 0 to 255.\nmax must be greater than min\n", "Invalid data error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}
