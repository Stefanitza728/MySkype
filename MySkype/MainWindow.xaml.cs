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
       

            var capture = new VideoCapture();
            var frame = new Mat();
            capture.ImageGrabbed += (s, ex) => {
                capture.Read(frame);
                var byteArray = ConvertorHelper.ConvertByteMapToByteArray(frame.Bitmap);
                DataComunication.SendImage(byteArray);
            };
            capture.Start();

            DataComunication.OpenCommunication(IpInput.Text, (bytes) => {
                this.Dispatcher.Invoke(() =>
                {
                    //MyImage.Source = ConvertorHelper.ConvertByteArrayToBitmapImage(bytes);
                });
            });
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            SetStatus("Closed");
            DataComunication.CloseCommunication();
        }
    }
}
