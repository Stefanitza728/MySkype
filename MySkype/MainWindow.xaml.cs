using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Emgu.CV;        
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

            DataComunication.OpenCommunication(IpInput.Text, (obj) => {
                this.Dispatcher.Invoke(() =>
                {
                    var img = (Bitmap)obj;
                    MyImage.Source = ConvertorHelper.ToBitmapImage(img);
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
