using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MyMusic.Wpf.Models
{
    public class Mp3View : INotifyPropertyChanged
    {
        private IEnumerable<Mp3File> _allFiles;
        public IEnumerable<Mp3File> AllFiles
        {
            get => _allFiles;
            set
            {
                _allFiles = value;
                NotifyOnProperyChanged();
            }
        }

        public TimeSpan LoadTime { get; set; }

        public ICommand PlayNextCommand { get; set; }

        public ICommand PlayAtEndCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyOnProperyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
