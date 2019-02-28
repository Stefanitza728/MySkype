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
        private readonly MainWindowViewModel _viewModel;
        protected MyDataComunication DataComunication { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataComunication = new MyDataComunication();
            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;
        }
        private void SetStatus(string status)
        {
            _viewModel.Status = status;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            SetStatus("Running");

            DataComunication.OpenCommunication(IpInput.Text, (obj) => {
                this.Dispatcher.Invoke(() =>
                {
                    var img = (Bitmap)obj;
                    _viewModel.ImageSource = ConvertorHelper.ToBitmapImage(img);
                });
            });
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            SetStatus("Closed");
            DataComunication.CloseCommunication();
            _viewModel.ImageSource = null;
        }
    }
}
