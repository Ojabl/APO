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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Win32;

namespace APO
{
    public partial class MainWindow : Window
    {
        public static OpenedImage imageWindow;
        public static Image<Bgr, byte> imgInput;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenImageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp";
            
            if (ofd.ShowDialog() == true)
            {
                imgInput = new Image<Bgr, byte>(ofd.FileName);

                imageWindow = new OpenedImage();
                imageWindow.imageSquare.Source = new BitmapImage(new Uri(ofd.FileName));
                imageWindow.Title = System.IO.Path.GetFileName(ofd.FileName);
                imageWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                imageWindow.Show();
            }
        }

        private void info_Click(object sender, RoutedEventArgs e)
        {
            Info info = new Info();
            info.Show();
        }
    }
}
