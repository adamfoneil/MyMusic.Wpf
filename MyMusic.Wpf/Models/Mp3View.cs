using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
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
                FilesCollection = CollectionViewSource.GetDefaultView(AllFiles);
            }
        }

        public ICollectionView FilesCollection { get; set; }

        public TimeSpan LoadTime
        {
            get => _loadTime;
            set
            {
                SetProperty(ref _loadTime, value);
            }
        }

        private string _searchText;

        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                SetProperty(ref _searchText, value);
                SearchFiles(_searchText);
            }
        }


        public ICommand PlayNextCommand { get; set; }

        public ICommand PlayAtEndCommand { get; set; }

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
                            return mp3File.IsSearchHit(text);
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
    }
}
