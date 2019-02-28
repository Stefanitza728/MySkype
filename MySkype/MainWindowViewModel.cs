using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MySkype
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public string status;
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChange("Status");
            }
        }

        public BitmapImage imageSource;
        public BitmapImage ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
                OnPropertyChange("ImageSource");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
