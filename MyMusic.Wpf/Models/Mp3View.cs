using MyMusic.Wpf.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MyMusic.Wpf.Models
{
    public enum Mp3ViewOption
    {
        /// <summary>
        /// The ListView
        /// </summary>
        ListView,

        /// <summary>
        /// The artist view
        /// </summary>
        ArtistView,

        /// <summary>
        /// The album view
        /// </summary>
        AlbumView

    }

    [TypeConverter(typeof(EnumDisplayTypeConverter))]
    public enum SortOptions
    {
        /// <summary>
        /// order by Artist asc, Album asc, Track asc
        /// </summary>
        [Display(Name = "Alphabetical")]
        Alphabetical,

        /// <summary>
        /// which artists are most represented in my collection? Beck, Sam Phillips, The Church
        /// order by Count(Artist) desc, Album asc, Track asc
        /// </summary>
        [Display(Name = "Most Represented")]
        MostRepresented,

        /// <summary>
        /// which artists are least represented in my collection? these are one-offs like Alanah Myles, John Prine, Tori Amos
        /// order by Count(Artist) asc, Artist asc, Album asc, Track asc
        /// </summary>
        [Display(Name = "Least Represented")]
        LeastRepresented,

        /// <summary>        
        /// order by DateAdded desc, Album asc, Track asc
        /// </summary>
        [Display(Name = "Recently Added")]
        RecentlyAdded,

        /// <summary>
        /// order by LastPlayed desc, Album asc, Track asc
        /// </summary>
        [Display(Name = "Recently Played")]
        RecentlyPlayed
    }

    public class Mp3View : BindableBase
    {
        private IEnumerable<Mp3File> _allFiles;
        private TimeSpan _loadTime;
        private bool _isLoaded;
        private IEnumerable<Mp3File> _filesCollection;
        private IEnumerable<object> _categoryCollection;
        private string _searchText = string.Empty;
        private SortOptions _currentSortOption;
        private Mp3ViewOption _currentView;

        public Mp3View()
        {
            SearchCommand = new DelegateCommand<object>((obj) => 
            {
                switch (obj)
                {
                    case AlbumGroup album:
                        CurrentView = Mp3ViewOption.ListView;
                        SearchText = $"album: {album.AlbumName.ToLower()}";                       
                        break;
                    case ArtistGroup artist:
                        CurrentView = Mp3ViewOption.ListView;
                        SearchText = $"artist: {artist.ArtistName.ToLower()}";                        
                        break;
                }
            });
        }

        public DelegateCommand<object> SearchCommand { get; set; }

        public IEnumerable<Mp3File> AllFiles
        {
            get => _allFiles;
            set
            {
                SetProperty(ref _allFiles, value);
                SearchFiles();
            }
        }

        public IEnumerable<Mp3File> FilesCollection
        {
            get => _filesCollection;
            set
            {
                SetProperty(ref _filesCollection, value);
            }
        }

        // This collection is used to bind to tile view. If we bind the FilesCollection to TileView binding errors throwing from Playlist control datagrid.
        public IEnumerable<object> CategoryCollection
        {
            get { return _categoryCollection; }
            set
            {
                SetProperty(ref _categoryCollection, value);
            }
        }


        public TimeSpan LoadTime
        {
            get => _loadTime;
            set
            {
                SetProperty(ref _loadTime, value);
                if(value != TimeSpan.Zero)
                {
                    IsLoaded = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is loaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is loaded; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoaded
        {
            get { return _isLoaded; }
            set
            {
                SetProperty(ref _isLoaded, value);
            }
        }        

        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                SetProperty(ref _searchText, value);
                SearchFiles();
            }
        }


        /// <summary>
        /// Gets or sets the current view.
        /// </summary>
        public Mp3ViewOption CurrentView
        {
            get => _currentView; 
            set => SetProperty(ref _currentView, value);
        }

        /// <summary>
        /// Gets or sets the current sort option.
        /// </summary>
        public SortOptions CurrentSortOption
        {
            get { return _currentSortOption; }
            set
            {
                SetProperty(ref _currentSortOption, value);
                SearchFiles();
            }
        }


        private void SearchFiles()
        {
            // Chooses the view based on view option.
            switch (CurrentView)
            {
                case Mp3ViewOption.ListView:
                    GenerateListView();
                    break;
                case Mp3ViewOption.ArtistView:
                    GroupByArtist();
                    break;
                case Mp3ViewOption.AlbumView:
                    GroupByAlbum();
                    break;
            }
        }

        public void GroupByArtist()
        {
            var sortedFiles = SortRules[_currentSortOption].Invoke(_allFiles).AsEnumerable();
            List<ArtistGroup> artistGroup = sortedFiles.Where(mp3File => mp3File.IsSearchHit(SearchText))
                .GroupBy(i => i.Artist).Select(i => new ArtistGroup { ArtistName = i.Key, Mp3Files = i.ToList() }).ToList();
            CategoryCollection = artistGroup;
        }

        public void GroupByAlbum()
        {
            var sortedFiles = SortRules[_currentSortOption].Invoke(_allFiles).AsEnumerable();
            List<AlbumGroup> albumGroup = sortedFiles.Where(mp3File => mp3File.IsSearchHit(SearchText))
                .GroupBy(i => (
                    i.Album,
                    i.Artist
                )).Select(i => new AlbumGroup
                {
                    AlbumName = i.Key.Album,
                    ArtistName = i.Key.Artist,
                    Mp3Files = i.ToList()
                }).ToList();
            CategoryCollection = albumGroup;
        }

        public void GenerateListView()
        {
            var sortedFiles = SortRules[_currentSortOption].Invoke(_allFiles).AsEnumerable();
            FilesCollection = sortedFiles.Where(mp3File => mp3File.IsSearchHit(SearchText));
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
