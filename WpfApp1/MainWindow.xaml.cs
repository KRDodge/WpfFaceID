using System;
using System.IO;
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
using Microsoft.Win32;
using WpfFaceID.API;

namespace WpfFaceID
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void ImageDrop(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files.Length > 1)
                    Console.WriteLine("only one file availible");

                string file = files[0];
                setFaceImage(file);
                drawFaceRectangle(file);
            }
        }

        public void GetImageByFileDialog(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP; *.JPG; *.GIF)| *.BMP; *.JPG; *.GIF | All files(*.*) | *.*";
            if (openFileDialog.ShowDialog() == true)
                FilePathTextBox.Text = openFileDialog.FileName;

            setFaceImage(openFileDialog.FileName);
            drawFaceRectangle(openFileDialog.FileName);
        }

        public void GetImageByFilePath(object sender, RoutedEventArgs e)
        {
            if(e.Source != null)
            {
                string file = FilePathTextBox.Text;

                setFaceImage(file);
                drawFaceRectangle(file);
            }
        }

        private void setFaceImage(string _filePath)
        {
            bool fileExist = File.Exists(_filePath);
            string extension = System.IO.Path.GetExtension(_filePath);
            bool checkExtension = checkImageExtension(extension);

            if (fileExist == false && checkExtension == false)
                return;

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(_filePath);
            bitmap.EndInit();

            Image image = new Image();
            image.Stretch = Stretch.None;
            image.Source = bitmap;

            image = resizeImage(image);

            FaceStackCanvas.Width = image.Width;
            FaceStackCanvas.Height = image.Height;

            if (FaceStackCanvas.Children.Count > 0)
                FaceStackCanvas.Children.Clear();

            FaceStackCanvas.Children.Add(image);
        }

        private Image resizeImage(Image _image)
        {
            if (_image.Width < 1024 && _image.Height < 1024)
                return _image;
           
            int max = _image.Width <= _image.Height ? (int)_image.Width : (int)_image.Height;
            float ratio = (float)1024 / (float)max;
            _image.Width = _image.Width * ratio;
            _image.Height = _image.Height * ratio;

            return _image;
        }

        private void drawFaceRectangle(string _filepath)
        {
            FaceRecognitionAPI api = new FaceRecognitionAPI();
            List<List<int>> rectanglePlots =  api.GetFaceRectangleList(_filepath);
            if (rectanglePlots.Count < 1)
                return;

            for(int i = 0; i < rectanglePlots.Count; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Stroke = Brushes.Red;
                rect.StrokeThickness = 2;
                Canvas.SetLeft(rect, rectanglePlots[i][0]);
                Canvas.SetTop(rect, rectanglePlots[i][1]);
                rect.Width = rectanglePlots[i][2];
                rect.Height = rectanglePlots[i][3];

                FaceStackCanvas.Children.Add(rect);
            }
        }

        private bool checkImageExtension(string _ext)
        {
            if(_ext == ".jpg" ||
                _ext == ".jpeg"||
                _ext == ".png" ||
                _ext == ".gif" ||
                _ext == ".tiff" ||
                _ext == ".tif" |
                _ext == "bmp")
                return true;

            return false;
        }
    }
}
