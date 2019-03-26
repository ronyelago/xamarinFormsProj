using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AppEpi.ViewModels
{
    public class MasterPageItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyPropertyChanged();
            }
        }

        public Type TargetType
        {
            get
            {
                return _targetType;
            }
            set
            {
                _targetType = value;
                NotifyPropertyChanged();
            }
        }

        private string _title;
        private Type _targetType;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
