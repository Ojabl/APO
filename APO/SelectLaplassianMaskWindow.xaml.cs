using APO_Projekt_1;
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
    public partial class SelectLaplassianMaskWindow : Window
    {
        public SelectLaplassianMaskWindow()
        {
            InitializeComponent();
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            int maskType;

            if (RadioButton1.IsChecked == true)
            {
                maskType = 1;
            }
            else if (RadioButton2.IsChecked == true)
            {
                maskType = 2;
            }
            else if (RadioButton3.IsChecked == true)
            {
                maskType = 3;
            }
            else
            {
                maskType = 1;
            }

            MainWindow.imgInput = Lab3.ApplyLaplassianMask(MainWindow.imgInput, maskType);
            
            this.Close();
        }
    }
}
