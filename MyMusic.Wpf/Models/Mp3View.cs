using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MyMusic.Wpf.Models
{
    public class Mp3View : BindableBase
    {
        private IEnumerable<Mp3File> _allFiles;
        private TimeSpan _loadTime;

        public IEnumerable<Mp3File> AllFiles
        {
            get => _allFiles;
            set
            {
                SetProperty(ref _allFiles, value);
            }
        }

        public TimeSpan LoadTime
        {
            get => _loadTime;
            set
            {
                SetProperty(ref _loadTime, value);
            }
        }
    }
}
