using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarkovPingTester
{
    class ServerPingData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _ServerRegion;
        public string ServerRegion { get
            {
                return _ServerRegion;
            }
            set
            {
                _ServerRegion = value;
                OnPropertyChanged("ServerRegion");
            }
        }

        private string _ServerName;
        public string ServerName
        {
            get
            {
                return _ServerName;
            }
            set
            {
                _ServerName = value;
                OnPropertyChanged("ServerName");
            }
        }

        private string[] _ServerAddress;
        public string[] ServerAddress
        {
            get
            {
                return _ServerAddress;
            }
            set
            {
                _ServerAddress = value;
                OnPropertyChanged("ServerAddress");
            }
        }


        private decimal _MinPing;
        public decimal MinPing
        {
            get
            {
                return _MinPing;
            }
            set
            {
                _MinPing = value;
                OnPropertyChanged("MinPing");
            }
        }

        private decimal _AvgPing;
        public decimal AvgPing
        {
            get
            {
                return _AvgPing;
            }
            set
            {
                _AvgPing = value;
                OnPropertyChanged("AvgPing");
            }
        }

        private decimal _MaxPing;
        public decimal MaxPing
        {
            get
            {
                return _MaxPing;
            }
            set
            {
                _MaxPing = value;
                OnPropertyChanged("MaxPing");
            }
        }

        private int _TestCount;
        public int TestCount
        {
            get
            {
                return _TestCount;
            }
            set
            {
                _TestCount = value;
                OnPropertyChanged("TestCount");
            }
        }

        private int _ErrorCount;
        public int ErrorCount
        {
            get
            {
                return _ErrorCount;
            }
            set
            {
                _ErrorCount = value;
                OnPropertyChanged("ErrorCount");
            }
        }


        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
