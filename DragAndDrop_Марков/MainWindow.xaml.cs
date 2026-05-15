using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace DragAndDrop_Марков
{
    public partial class MainWindow : Window
    {
        private BitmapSource originalImage;
        private Point startPoint;
        private bool isDragging = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (openDialog.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openDialog.FileName);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                originalImage = bitmap;
                MainImage.Source = originalImage;
                CropRect.Visibility = Visibility.Collapsed;
            }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MainImage.Source == null) return;

            isDragging = true;
            startPoint = e.GetPosition(MainImage);

            Canvas.SetLeft(CropRect, startPoint.X);
            Canvas.SetTop(CropRect, startPoint.Y);
            CropRect.Width = 0;
            CropRect.Height = 0;
            CropRect.Visibility = Visibility.Visible;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging || MainImage.Source == null) return;

            Point currentPoint = e.GetPosition(MainImage);

            double width = currentPoint.X - startPoint.X;
            double height = currentPoint.Y - startPoint.Y;

            if (width < 0)
            {
                Canvas.SetLeft(CropRect, currentPoint.X);
                width = -width;
            }

            if (height < 0)
            {
                Canvas.SetTop(CropRect, currentPoint.Y);
                height = -height;
            }

            CropRect.Width = width;
            CropRect.Height = height;
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
        }

        private void CropButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainImage.Source == null || CropRect.Visibility != Visibility.Visible)
            {
                MessageBox.Show("Щакализатор должен получить фото и увидеть область", "Косяк");
                return;
            }

            if (CropRect.Width <= 0 || CropRect.Height <= 0)
            {
                MessageBox.Show("Шакализатор должен что-то видеть", "Косяк");
                return;
            }

            try
            {
                double imageWidth = originalImage.PixelWidth;
                double imageHeight = originalImage.PixelHeight;

                double actualWidth = MainImage.ActualWidth;
                double actualHeight = MainImage.ActualHeight;

                double scaleX = imageWidth / actualWidth;
                double scaleY = imageHeight / actualHeight;

                int cropX = (int)(Canvas.GetLeft(CropRect) * scaleX);
                int cropY = (int)(Canvas.GetTop(CropRect) * scaleY);
                int cropWidth = (int)(CropRect.Width * scaleX);
                int cropHeight = (int)(CropRect.Height * scaleY);

                CroppedBitmap cropped = new CroppedBitmap(originalImage,
                    new Int32Rect(cropX, cropY, cropWidth, cropHeight));

                MainImage.Source = cropped;
                originalImage = cropped;
                CropRect.Visibility = Visibility.Collapsed;

                MessageBox.Show("Фото зашакалено!", "Чиназес");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка шакалирования: {ex.Message}", "Косяк");
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            if (originalImage != null)
            {
                LoadImageButton_Click(null, null);
            }
        }
    }
}