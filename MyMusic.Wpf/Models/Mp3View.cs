using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace MyMusic.Wpf.Models
{
    public enum SortOptions
    {
        /// <summary>
        /// order by Artist asc, Album asc, Track asc
        /// </summary>
        Alphabetical,
        /// <summary>
        /// which artists are most represented in my collection? Beck, Sam Phillips, The Church
        /// order by Count(Artist) desc, Album asc, Track asc
        /// </summary>
        MostRepresented,
        /// <summary>
        /// which artists are least represented in my collection? these are one-offs like Alanah Myles, John Prine, Tori Amos
        /// order by Count(Artist) asc, Artist asc, Album asc, Track asc
        /// </summary>
        LeastRepresented,
        /// <summary>        
        /// order by DateAdded desc, Album asc, Track asc
        /// </summary>
        RecentlyAdded,
        /// <summary>
        /// order by LastPlayed desc, Album asc, Track asc
        /// </summary>
        RecentlyPlayed
    }

    public class Mp3View : BindableBase
    {
        private IEnumerable<Mp3File> _allFiles;
        private TimeSpan _loadTime;
        private ICollectionView _filesCollection;

        public IEnumerable<Mp3File> AllFiles
        {
            get => _allFiles;
            set
            {
                SetProperty(ref _allFiles, value);
                FilesCollection = CollectionViewSource.GetDefaultView(AllFiles);
            }
        }

        public ICollectionView FilesCollection
        {
            get => _filesCollection;
            set
            {
                SetProperty(ref _filesCollection, value);
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

        public static Dictionary<SortOptions, Func<IEnumerable<Mp3File>, IOrderedEnumerable<Mp3File>>> SortRules => new Dictionary<SortOptions, Func<IEnumerable<Mp3File>, IOrderedEnumerable<Mp3File>>>()
        {
            [SortOptions.Alphabetical] = (list) => list.OrderBy(file => file.Artist).ThenBy(file => file.Album).ThenBy(file => file.TrackNumber),
            [SortOptions.MostRepresented] = (list) => ArtistOrdering(list, (artistGrp) => artistGrp.OrderByDescending(grp => grp.Count())),
            [SortOptions.LeastRepresented] = (list) => ArtistOrdering(list, (artistGrp) => artistGrp.OrderBy(grp => grp.Count())),
            [SortOptions.RecentlyAdded] = (list) => list.OrderByDescending(file => file.DateAdded),
            [SortOptions.RecentlyPlayed] = (list) => list.OrderByDescending(file => file.LastPlayed)
        };

        private static IOrderedEnumerable<Mp3File> ArtistOrdering(IEnumerable<Mp3File> files, Func<IEnumerable<IGrouping<string, Mp3File>>, IOrderedEnumerable<IGrouping<string, Mp3File>>> artistOrder)
        {
            var grouped = artistOrder.Invoke(files.GroupBy(file => file.Artist)).ToList();

            var results = new List<Mp3File>();
            grouped.ForEach(artistGrp => results.AddRange(artistGrp.OrderBy(file => file.Album).ThenBy(file => file.TrackNumber)));

            // thanks to https://stackoverflow.com/a/14404330/2023653
            return results.OrderBy(file => 1);
        }
    }
}
