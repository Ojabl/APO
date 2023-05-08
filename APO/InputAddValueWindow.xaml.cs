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
    public partial class InputAddValueWindow : Window
    {
        public int value;

        public InputAddValueWindow()
        {
            InitializeComponent();
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            this.value = int.Parse(this.TbValue.Text);
            Image<Bgr,byte> addedImage =  Lab3.MathAdd(MainWindow.imgInput, this.value);
            
            MainWindow.imgInput = addedImage;

            OpenedImage openedImage = new OpenedImage()
            {
                imageSquare = { Source = addedImage.ToBitmapSource() },
                Title = "Image after addition",
            };
            openedImage.Show();


            this.Close();
        }
    }
}
