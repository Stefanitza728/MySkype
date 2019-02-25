using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Emgu.CV;
using MySkypeCommon;
using NAudio.Wave;

namespace MySkype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected MyDataComunication DataComunication { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataComunication = new MyDataComunication();
        }
        private void SetStatus(string status)
        {
            this.StatusLabel.Content = status;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            SetStatus("Running");
            DataComunication.OpenCommunication(IpInput.Text);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            SetStatus("Closed");
            DataComunication.CloseCommunication();
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            
        }

        private void CameraCapture_Click(object sender, RoutedEventArgs e)
        {
            var capture = new VideoCapture();
            capture.ImageGrabbed += (s,ex) => {
                if (capture != null && capture.Ptr != IntPtr.Zero)
                {
                    MyImage.Source = BitmapToImageSource(capture.QueryFrame().Bitmap);
                }
            };
            capture.Start();
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
