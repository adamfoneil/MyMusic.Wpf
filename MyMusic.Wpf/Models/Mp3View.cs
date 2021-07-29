using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

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
        private IEnumerable<object> _filesCollection;

        public IEnumerable<Mp3File> AllFiles
        {
            get => _allFiles;
            set
            {
                SetProperty(ref _allFiles, value);
                FilesCollection = _allFiles;
            }
        }

        public IEnumerable<object> FilesCollection
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

        private string _searchText = string.Empty;

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

        public Mp3ViewOption CurrentView { get; set; }

        private void SearchFiles()
        {
            // Applyting filter based on search text.
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
            CurrentView = Mp3ViewOption.ArtistView;
            List<ArtistGroup> artistGroup = _allFiles.Where(mp3File => mp3File.IsSearchHit(SearchText))
                .GroupBy(i => i.Artist).Select(i => new ArtistGroup { ArtistName = i.Key, Mp3Files = i.ToList() }).ToList();
            FilesCollection = artistGroup;
        }

        public void GroupByAlbum()
        {
            CurrentView = Mp3ViewOption.AlbumView;
            List<AlbumGroup> albumGroup = _allFiles.Where(mp3File =>  mp3File.IsSearchHit(SearchText))
                .GroupBy(i => (
                    i.Album,
                    i.Artist
                )).Select(i => new AlbumGroup
                {
                    AlbumName = i.Key.Album,
                    ArtistName = i.Key.Artist,
                    Mp3Files = i.ToList()
                }).ToList();
            FilesCollection = albumGroup;
        }

        public void GenerateListView()
        {
            CurrentView = Mp3ViewOption.ListView;
            FilesCollection = _allFiles.Where(mp3File => mp3File.IsSearchHit(SearchText));
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
