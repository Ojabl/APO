using Emgu.CV;
using Emgu.CV.Structure;
using System.Windows;
using static APO.OpenedImage;

namespace APO
{
    public partial class InputSingleValueWindow : Window
    {
        public static int value;
        MathOperation mathOperation;

        public InputSingleValueWindow()
        {
            InitializeComponent();
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            value = int.Parse(this.TbValue.Text);
            mathOperation = OpenedImage.mathOperation;
            Image<Bgr, byte> resultImage = new Image<Bgr,byte>(MainWindow.imgInput.Size);

            switch (mathOperation)
            {
                case MathOperation.Addition:
                    resultImage = Lab3.MathAddition(MainWindow.imgInput, value);
                    break;

                case MathOperation.Subtraction:
                    resultImage = Lab3.MathSubtraction(MainWindow.imgInput, value);
                    break;
            }

            OpenedImage imageAfterMathOperation = new OpenedImage()
            {
                imageSquare = { Source = resultImage.ToBitmapSource() },
                Title = "Image after " + mathOperation.ToString() + " operation",
            };
            imageAfterMathOperation.Show();

            this.Close();
        }
    }
}
