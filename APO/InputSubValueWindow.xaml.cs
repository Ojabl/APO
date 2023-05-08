using Emgu.CV;
using Emgu.CV.Structure;
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
    public partial class InputSubValueWindow : Window
    {
        public int value;

        public InputSubValueWindow()
        {
            InitializeComponent();
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            this.value = int.Parse(this.TbValue.Text);
            Image<Bgr,byte> SubtractedImage = Lab3.MathSub(MainWindow.imgInput, this.value);
            
            MainWindow.imgInput = SubtractedImage;

            OpenedImage openedimage = new OpenedImage()
            {
                imageSquare = { Source = SubtractedImage.ToBitmapSource() },
                Title = "Image after subtraction",
            };
            openedimage.Show();
            this.Close();
        }
    }
}
