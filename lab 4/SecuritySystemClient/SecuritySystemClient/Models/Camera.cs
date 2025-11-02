using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SecuritySystem.Models
{
    public class Camera : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _location;
        private bool _isActive;
        private string _status;
        private string _streamUrl;

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

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Location
        {
            get { return _location; }
            set
            {
                if (_location != value)
                {
                    _location = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(StatusText));
                }
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StreamUrl
        {
            get { return _streamUrl; }
            set
            {
                if (_streamUrl != value)
                {
                    _streamUrl = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StatusText => IsActive ? "Активна" : "Неактивна";

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}