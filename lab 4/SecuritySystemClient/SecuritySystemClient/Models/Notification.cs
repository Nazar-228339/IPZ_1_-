using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SecuritySystem.Models
{
    public class Notification : INotifyPropertyChanged
    {
        private int _id;
        private string _title;
        private string _message;
        private DateTime _timestamp;
        private string _severity;
        private bool _isRead;
        private int _cameraId;

        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Timestamp
        {
            get { return _timestamp; }
            set
            {
                if (_timestamp != value)
                {
                    _timestamp = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TimeString));
                }
            }
        }

        public string Severity
        {
            get { return _severity; }
            set
            {
                if (_severity != value)
                {
                    _severity = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsRead
        {
            get { return _isRead; }
            set
            {
                if (_isRead != value)
                {
                    _isRead = value;
                    OnPropertyChanged();
                }
            }
        }

        public int CameraId
        {
            get { return _cameraId; }
            set
            {
                if (_cameraId != value)
                {
                    _cameraId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TimeString => Timestamp.ToString("dd.MM.yyyy HH:mm:ss");

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}