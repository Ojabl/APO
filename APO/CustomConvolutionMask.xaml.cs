using Emgu.CV;
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

namespace APO
{
    public partial class CustomConvolutionMask : Window
    {
        public CustomConvolutionMask()
        {
            InitializeComponent();
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int a1 = Convert.ToInt32(TbCustomMatrixA1.Text);
                int a2 = Convert.ToInt32(TbCustomMatrixA2.Text);
                int a3 = Convert.ToInt32(TbCustomMatrixA3.Text);
                int b1 = Convert.ToInt32(TbCustomMatrixB1.Text);
                int b2 = Convert.ToInt32(TbCustomMatrixB2.Text);
                int b3 = Convert.ToInt32(TbCustomMatrixB3.Text);
                int c1 = Convert.ToInt32(TbCustomMatrixC1.Text);
                int c2 = Convert.ToInt32(TbCustomMatrixC2.Text);
                int c3 = Convert.ToInt32(TbCustomMatrixC3.Text);
           
                int[,] customMask = new int[,]
                {
                    { a1, a2, a3 },
                    { b1, b2, b3 },
                    { c1, c2, c3 }

                };

                var resultImage = Lab3.ApplyCustomMask(MainWindow.imgInput, customMask);
                MainWindow.imgInput = resultImage;

                OpenedImage openedImage = new OpenedImage()
                {
                    Title = "Custom convolution mask",
                    imageSquare = { Source = resultImage.ToBitmapSource() }
                };
                openedImage.Show();
                this.Close();
            }
            catch { MessageBox.Show("Please input valid data\nevery cell of convolution mask must be a full number\ncorrect the input and try again", "Invalid convolution mask error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}
