using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
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

        /// <summary>
        /// I thought DataGrid would bind to this to provide searchable list.
        /// </summary>
        public ICollectionView FilesCollection => CollectionViewSource.GetDefaultView(AllFiles);

        public TimeSpan LoadTime { get; set; }

        private string _searchText;

        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                SearchFiles(_searchText);
                NotifyOnProperyChanged();
            }
        }


        public ICommand PlayNextCommand { get; set; }

        public ICommand PlayAtEndCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SearchFiles(string text)
        {
            // Applyting filter based on search text.
            if (FilesCollection != null)
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    FilesCollection.Filter = (item) =>
                    {
                        if (item is Mp3File mp3File)
                        {
                            // Filter comparison based on Title, Artist, Album, and Filename
                            return mp3File.Title.ToLower().Contains(text.ToLower()) || mp3File.Artist.ToLower().Contains(text.ToLower())
                            || mp3File.Album.ToLower().Contains(text.ToLower()) || mp3File.Filename.ToLower().Contains(text.ToLower());
                        }
                        return false;
                    };
                }
                else // Resetting the filter.
                {
                    FilesCollection.Filter = null;
                }
            }
        }

        public void NotifyOnProperyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
