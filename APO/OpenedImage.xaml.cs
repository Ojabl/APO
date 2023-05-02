using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using APO_Projekt_1;
using Emgu.CV;
using Emgu.CV.Structure;
using ScottPlot;

namespace APO
{
    public partial class OpenedImage : Window
    {
        public Image image;
        public bool drawPlotProfileLine = false;
        
        private Point PlotProfilestartPoint;
        private Point PlotProfileendPoint;

        public OpenedImage()
        {
            InitializeComponent();

            #region plot profile mouse tracking

            imageSquare.MouseLeftButtonDown += ImageSquare_MouseLeftButtonDown;
            imageSquare.MouseLeftButtonUp += ImageSquare_MouseLeftButtonUp;
            imageSquare.MouseMove += ImageSquare_MouseMove;
            
            #endregion
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            MainWindow.imgInput = ParseHelper.ConvertImageSourceToEmguImage(this.imageSquare.Source);
        }

        private void DuplicateWindow_Click(object sender, RoutedEventArgs e)
        {
            if(MainWindow.imageWindow != null)
            {
                BitmapSource currentImage = imageSquare.Source as BitmapSource;

                OpenedImage duplicatedImageWindow = new OpenedImage
                {
                    imageSquare = { Source = currentImage},
                    Title = "Duplicated Image - " + System.IO.Path.GetFileName(MainWindow.imageWindow.Title)
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
            List<Mat> channelsHSV = Lab1.SplitHSVChannels();

            if (channelsHSV != null)
            {
                List<BitmapSource> bitmapSources = new List<BitmapSource>();

                foreach (Mat mat in channelsHSV)
                {
                    bitmapSources.Add(mat.ToBitmapSource());
                }
                
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
            List<Mat> channelsLAB = Lab1.SplitLABChannels();

            if (channelsLAB != null && channelsLAB.Count >= 3)
            {
                OpenedImage channel1Window = new OpenedImage
                {
                    imageSquare = { Source = channelsLAB[0].ToBitmapSource() },
                    Title = "Channel 1 - " + System.IO.Path.GetFileName(MainWindow.imageWindow.Title)
                };
                OpenedImage channel2Window = new OpenedImage
                {
                    imageSquare = { Source = channelsLAB[1].ToBitmapSource() },
                    Title = "Channel 2 - " + System.IO.Path.GetFileName(MainWindow.imageWindow.Title)
                };
                OpenedImage channel3Window = new OpenedImage
                {
                    imageSquare = { Source = channelsLAB[2].ToBitmapSource() },
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

            for (int x = 0; x < MainWindow.imgInput.Height; x++)
            {
                for (int y = 0; y < MainWindow.imgInput.Width; y++)
                {
                    Bgr pixel = MainWindow.imgInput[x,y];
                    int gray = (int)Math.Round(((pixel.Red * 0.299) + (pixel.Green * 0.587) + (pixel.Blue * 0.114)));
                    histogramValues[gray]++;
                }
            }

            double[] histogramValuesDouble = histogramValues.Select(v => (double)v).ToArray();
            double[] positions = Enumerable.Range(0, 256).Select(i => (double)i).ToArray();

            Histogram histogramWindow = new Histogram();
            histogramWindow.Title = "Histogram";
            var wpfPlot = new WpfPlot();

            wpfPlot.Plot.PlotBar(positions, histogramValuesDouble);
            wpfPlot.Plot.Title("Histogram");
            wpfPlot.Plot.XLabel("Grayscale Value");
            wpfPlot.Plot.YLabel("Count");

            wpfPlot.MouseMove += (s, eMouseMove) =>
            {
                var mousePos = eMouseMove.GetPosition(this);
                double mouseX = wpfPlot.Plot.GetCoordinateX((float)mousePos.X);
                double mouseY = wpfPlot.Plot.GetCoordinateY((float)mousePos.Y);
                long xValue = (long)Math.Round(mouseX);

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


        #region plot profile line drawing
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

                imageSquare.CaptureMouse();
            }
        }
        private void ImageSquare_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (drawPlotProfileLine)
            {
                PlotProfileendPoint = e.GetPosition(imageSquare);

                double[] intensityProfile = Lab1.ComputeIntensityProfile(MainWindow.imgInput, PlotProfilestartPoint, PlotProfileendPoint);
                Lab1.DisplayIntensityProfile(intensityProfile, PlotProfilestartPoint, PlotProfileendPoint);

                imageSquare.ReleaseMouseCapture();
            }
            drawPlotProfileLine = false;
        }
        private void ImageSquare_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && drawPlotProfileLine)
            {
                System.Windows.Point currentPoint = e.GetPosition(this);
                PlotProfileLine.X2 = currentPoint.X;
                PlotProfileLine.Y2 = currentPoint.Y;
            }
        }
        #endregion


        private void HistogramStretch_Click(object sender, RoutedEventArgs e)
        {
            HistogramInputValues hivWindow = new HistogramInputValues();
            hivWindow.Show();
        }
        private void HistogramEqualization_Click(object sender, RoutedEventArgs e)
        {
            this.imageSquare.Source = Lab2.Equalization(MainWindow.imgInput).ToBitmapSource();
        }
        
        private void Negation_Click(object sender, RoutedEventArgs e)
        {
            this.imageSquare.Source = Lab2.InvertColors(imageSquare.Source as BitmapSource);
        }
        
        private void Posterize_Click(object sender, RoutedEventArgs e)
        {
            PosterizeInputValuesWindow posterizeWindow = new PosterizeInputValuesWindow();
            posterizeWindow.Show();
        }

        #region Neighbourhood operations
        private void GaussianBlur_Click(object sender, RoutedEventArgs e)
        {
            this.imageSquare.Source = Lab3.GaussianBlur(MainWindow.imgInput, 5, 1.0).ToBitmapSource();
        }
        private void LaplassianSharpening_Click(object sender, RoutedEventArgs e)
        {
            int maskType = 1; // Change this value to 1, 2, or 3 for different Laplacian masks
            this.imageSquare.Source = Lab3.ApplyLaplassianMask(MainWindow.imgInput, maskType).ToBitmapSource();
        }

        private void SobelEdgeDetection_Click(object sender, RoutedEventArgs e)
        {
            this.imageSquare.Source = Lab3.SobelEdgeDetection(MainWindow.imgInput).ToBitmapSource();
        }
        private void LaplacianEdgeDetection_Click(object sender, RoutedEventArgs e)
        {
            this.imageSquare.Source = Lab3.LaplacianEdgeDetection(MainWindow.imgInput).ToBitmapSource();
        }
        private void CannyEdgeDetection_Click(object sender, RoutedEventArgs e)
        {
            this.imageSquare.Source = Lab3.CannyEdgeDetection(MainWindow.imgInput, 50, 150).ToBitmapSource();
        }

        private void LaplacianMask_Click(object sender, RoutedEventArgs e)
        {
            SelectLaplassianMaskWindow selectLaplassianMaskWindow = new SelectLaplassianMaskWindow();
            selectLaplassianMaskWindow.Show();
        }
        #endregion
    }
}