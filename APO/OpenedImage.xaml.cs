using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;
using ScottPlot;


namespace APO
{
    public partial class OpenedImage : Window
    {
        private Image image;
        public bool drawPlotProfileLine = false;
        
        private Point PlotProfilestartPoint;
        private Point PlotProfileendPoint;

        public OpenedImage()
        {
            InitializeComponent();
            imageSquare.MouseLeftButtonDown += ImageSquare_MouseLeftButtonDown;
            imageSquare.MouseLeftButtonUp += ImageSquare_MouseLeftButtonUp;
            imageSquare.MouseMove += ImageSquare_MouseMove;
        }

        private void DuplicateWindow_Click(object sender, RoutedEventArgs e)
        {
            if(MainWindow.imageWindow != null)
            {
                BitmapSource currentImage = imageSquare.Source as BitmapSource;
                Image duplicatedImage = new Image();

                OpenedImage duplicatedImageWindow = new OpenedImage
                {
                    imageSquare = { Source = currentImage},
                    Title = "Duplicated Image" + System.IO.Path.GetFileName(MainWindow.imageWindow.Title)
                };
                
                duplicatedImageWindow.Show();
            }
        }

        private void Grayscale_Click(object sender, RoutedEventArgs e)
        {
            this.imageSquare.Source = Lab1.ConvertToGrayscale().ToBitmapSource();
        }

        private void HSV_Click(object sender, RoutedEventArgs e)
        {
            this.imageSquare.Source = Lab1.ConvertToHSV().ToBitmapSource();
        }

        private void Lab_Click(object sender, RoutedEventArgs e)
        {
            this.imageSquare.Source = Lab1.ConvertToLab().ToBitmapSource();
        }

        private void ThreeHSVChannels_Click(object sender, RoutedEventArgs e)
        {
            List<Mat> mats = Lab1.SplitHSVChannels();

            if (mats != null)
            {
                List<BitmapSource> bitmapSources = new List<BitmapSource>();

                foreach (Mat mat in mats)
                {
                    bitmapSources.Add(mat.ToBitmapSource());
                }

                // Now you have a list of BitmapSource objects that you can display in your windows
                
                if (bitmapSources.Count >= 3)
                {
                    OpenedImage channel1Window = new OpenedImage
                    {
                        imageSquare = { Source = bitmapSources[0] },
                        Title = "Channel 1 - " + System.IO.Path.GetFileName(MainWindow.imageWindow.Title)
                    };
                    OpenedImage channel2Window = new OpenedImage
                    {
                        imageSquare = { Source = bitmapSources[1] },
                        Title = "Channel 2 - " + System.IO.Path.GetFileName(MainWindow.imageWindow.Title)
                    };
                    OpenedImage channel3Window = new OpenedImage
                    {
                        imageSquare = { Source = bitmapSources[2] },
                        Title = "Channel 3 - " + System.IO.Path.GetFileName(MainWindow.imageWindow.Title)
                    };

                    channel1Window.Show();
                    channel2Window.Show();
                    channel3Window.Show();
                }
            }
        }

        private void ThreeLABChannels_Click(object sender, RoutedEventArgs e)
        {
            var currentImage = imageSquare.Source as BitmapSource;

            List<Mat> labChannels = Lab1.ConvertToLabThreeChannels();

            if (labChannels != null && labChannels.Count >= 3)
            {
                OpenedImage channel1Window = new OpenedImage
                {
                    imageSquare = { Source = labChannels[0].ToBitmapSource() },
                    Title = "Channel 1 - " + System.IO.Path.GetFileName(MainWindow.imageWindow.Title)
                };
                OpenedImage channel2Window = new OpenedImage
                {
                    imageSquare = { Source = labChannels[1].ToBitmapSource() },
                    Title = "Channel 2 - " + System.IO.Path.GetFileName(MainWindow.imageWindow.Title)
                };
                OpenedImage channel3Window = new OpenedImage
                {
                    imageSquare = { Source = labChannels[2].ToBitmapSource() },
                    Title = "Channel 3 - " + System.IO.Path.GetFileName(MainWindow.imageWindow.Title)
                };

                channel1Window.Show();
                channel2Window.Show();
                channel3Window.Show();
            }
        }

        private void Histogram_Click(object sender, RoutedEventArgs e)
        {
            int[] histogramValues = new int[256];

            //liczenie histogramu(punktów)
            for (int y = 0; y < MainWindow.imgInput.Height; y++)
            {
                for (int x = 0; x < MainWindow.imgInput.Width; x++)
                {
                    Bgr pixel = MainWindow.imgInput[y, x];
                    byte gray = (byte)((pixel.Red * 0.299) + (pixel.Green * 0.587) + (pixel.Blue * 0.114));
                    histogramValues[gray]++;

                }
            }
            //konwersja 
            double[] histogramValuesDouble = histogramValues.Select(v => (double)v).ToArray();
            double[] positions = Enumerable.Range(0, 256).Select(i => (double)i).ToArray();
            //display
            Histogram histogramWindow = new Histogram();
            histogramWindow.Title = "Histogram";
            var wpfPlot = new WpfPlot();

            wpfPlot.Plot.PlotBar(positions, histogramValuesDouble);
            wpfPlot.Plot.Title("Histogram");
            wpfPlot.Plot.XLabel("Grayscale Value");
            wpfPlot.Plot.YLabel("Count");

            //wartości
            wpfPlot.MouseMove += (s, eMouseMove) =>
            {
                var mousePos = eMouseMove.GetPosition(wpfPlot);
                double mouseX = wpfPlot.Plot.GetCoordinateX((float)mousePos.X);
                double mouseY = wpfPlot.Plot.GetCoordinateY((float)mousePos.Y);
                long xValue = (long)Math.Round(mouseX);

                // walidacja 
                if (xValue >= 0 && xValue < 256)
                {
                    long yValue = (long)histogramValues[xValue];
                    histogramWindow.Title = $"Wartość: {yValue}";
                }
            };

            histogramWindow.histogramPlotContainer.Children.Add(wpfPlot);
            histogramWindow.Show();
            wpfPlot.Refresh();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            MainWindow.imgInput = ParseHelper.ConvertImageSourceToEmguImage(this.imageSquare.Source);
        }

        private double[] ComputeIntensityProfile(Image<Bgr, byte> image, System.Windows.Point startPoint, System.Windows.Point endPoint)
        {
            int numPoints = (int)Math.Ceiling(Math.Sqrt(Math.Pow(startPoint.X - endPoint.X, 2) + Math.Pow(startPoint.Y - endPoint.Y, 2)));
            double[] profile = new double[numPoints];

            for (int i = 0; i < numPoints; i++)
            {
                double t = (double)i / (numPoints - 1);
                int x = (int)Math.Round(startPoint.X * (1 - t) + endPoint.X * t);
                int y = (int)Math.Round(startPoint.Y * (1 - t) + endPoint.Y * t);
                Bgr pixel = image[y, x];
                byte gray = (byte)((pixel.Red * 0.299) + (pixel.Green * 0.587) + (pixel.Blue * 0.114));
                profile[i] = gray;
            }

            return profile;
        }
        private void DisplayIntensityProfile(double[] intensityProfile, Point startPoint, Point endPoint)
        {
            Histogram histogramWindow = new Histogram();
            histogramWindow.Title = "Plot Profile";
            var wpfPlot = new WpfPlot();

            double[] positions = Enumerable.Range(0, intensityProfile.Length).Select(i => (double)i).ToArray();
            wpfPlot.Plot.PlotScatter(positions, intensityProfile);
            wpfPlot.Plot.Title("Linia Profilu");
            wpfPlot.Plot.XLabel("Pozycja ");
            wpfPlot.Plot.YLabel("Intensywność");

            histogramWindow.histogramPlotContainer.Children.Add(wpfPlot);
            histogramWindow.Show();
            wpfPlot.Refresh();
        }
        
        private void PlotProfile_Click(object sender, EventArgs e)
        {
            drawPlotProfileLine = true;
        }
        private void ImageSquare_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (drawPlotProfileLine)
            {
                PlotProfilestartPoint = e.GetPosition(this);
                PlotProfileLine.X1 = PlotProfilestartPoint.X;
                PlotProfileLine.Y1 = PlotProfilestartPoint.Y;
                PlotProfileLine.X2 = PlotProfilestartPoint.X;
                PlotProfileLine.Y2 = PlotProfilestartPoint.Y;
                PlotProfileLine.Visibility = Visibility.Visible;

                // Capture the mouse to receive events even when the cursor is outside the control's bounds
                imageSquare.CaptureMouse();
            }
        }
        private void ImageSquare_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (drawPlotProfileLine)
            {
                PlotProfileendPoint = e.GetPosition(imageSquare);

                double[] intensityProfile = ComputeIntensityProfile(MainWindow.imgInput, PlotProfilestartPoint, PlotProfileendPoint);
                DisplayIntensityProfile(intensityProfile, PlotProfilestartPoint, PlotProfileendPoint);

                // Release the mouse capture
                imageSquare.ReleaseMouseCapture();
            }
            drawPlotProfileLine = false;
        }
        private void ImageSquare_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && drawPlotProfileLine)
            {
                System.Windows.Point currentPoint = e.GetPosition(imageSquare);
                PlotProfileLine.X2 = currentPoint.X;
                PlotProfileLine.Y2 = currentPoint.Y;
            }
        }

        private void HistogramStretch_Click(object sender, RoutedEventArgs e)
        {
            //this.imageSquare.Source = Lab2.Equalization(MainWindow.imgInput).ToBitmapSource(); // histogram stretch a nie equalization

            HistogramInputValues hivWindow = new HistogramInputValues
            {
                Title = "Histogram Stretch value input"
            };
            hivWindow.Show();
        }

        private void Negation_Click(object sender, RoutedEventArgs e)
        {
            var currentImage = imageSquare.Source as BitmapSource;
            var invertedImage = Lab2.InvertColors(currentImage);
            this.imageSquare.Source = invertedImage;
        }
    }
}