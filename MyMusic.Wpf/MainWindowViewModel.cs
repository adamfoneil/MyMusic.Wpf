using MyMusic.Wpf.Controls;
using MyMusic.Wpf.Models;
using MyMusic.Wpf.Services;
using MyMusic.Wpf.ViewModels;
using MyMusic.Wpf.Views;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Input;

namespace MyMusic.Wpf
{
    public class MainWindowViewModel : BindableBase
    {
        private MetadataCache _cache;
        private PlayerViewModel _playerViewModel;
        private SettingsViewModel _settingsViewModel;

        public MainWindowViewModel(MetadataCache cache, PlayerViewModel playerViewModel, Mp3View mp3View, SettingsViewModel settingsViewModel)
        {
            _cache = cache;
            _cache.Refreshed += MetadataCache_Refreshed;
            _playerViewModel = playerViewModel;
            _settingsViewModel = settingsViewModel;
            Mp3View = mp3View;
            RebuildCommand = new DelegateCommand(Rebuild);
            PlayNextCommand = new DelegateCommand<Mp3File>(mp3 => PlayNext(mp3));
            PlayAtEndCommand = new DelegateCommand<Mp3File>(mp3 => PlayAtEnd(mp3));
            SettingsCommand = new DelegateCommand<Window>(OpenSettingsView);
            ShowArtistCommand = new DelegateCommand(ShowArtistView);
            ShowAlbumCommand = new DelegateCommand(ShowAlbumView);
            ShowListCommand = new DelegateCommand(ShowListView);
            PlayCategoryCommand = new DelegateCommand<object>(PlayCategory);
            RescanCommand = new DelegateCommand<Mp3File>((currenTrack) => 
            {
                _cache.Rescan(currenTrack.FullPath);
            });
        }

        private void MetadataCache_Refreshed()
        {
            Rebuild();
        }

        public Mp3View Mp3View { get; set; }

        public ICommand RebuildCommand { get; set; }

        public ICommand PlayAtEndCommand { get; set; }

        public ICommand PlayNextCommand { get; set; }

        public ICommand RescanCommand { get; set; }

        public ICommand SettingsCommand { get; set; }

        public ICommand ShowArtistCommand { get; set; }

        public ICommand ShowAlbumCommand { get; set; }

        public ICommand ShowListCommand { get; set; }

        public ICommand PlayCategoryCommand { get; set; }

        public PlayerViewModel PlayerViewModel
        {
            get => _playerViewModel;
            set
            {
                SetProperty(ref _playerViewModel, value);
            }
        }

        private void OpenSettingsView(Window obj)
        {
            SettingsView settingsView = new SettingsView(_settingsViewModel);
            settingsView.Owner = obj;
            var result = settingsView.ShowDialog();
            if (result.HasValue && result.Value)
            {
                Rebuild();
            }
        }


        private void PlayNext(Mp3File mp3)
        {
            _playerViewModel?.PlayNext(mp3);
        }

        private void PlayAtEnd(Mp3File mp3)
        {
            _playerViewModel.PlayAtEnd(mp3);
        }

        public async void LoadFiles()
        {
            Mp3View.AllFiles = await _cache.GetMp3FilesAsync();
            Mp3View.LoadTime = _cache.ScanTime;
        }

        public async void Rebuild()
        {
            Mp3View.AllFiles = await _cache.GetMp3FilesAsync(rebuild: true);
            Mp3View.LoadTime = _cache.ScanTime;
        }

        private void ShowListView()
        {
            Mp3View.CurrentView = Mp3ViewOption.ListView;
            Mp3View?.GenerateListView();
        }

        private void ShowAlbumView()
        {
            Mp3View.CurrentView = Mp3ViewOption.AlbumView;
            Mp3View?.GroupByAlbum();
        }

        private void ShowArtistView()
        {
            Mp3View.CurrentView = Mp3ViewOption.ArtistView;
            Mp3View?.GroupByArtist();
        }

        private void PlayCategory(object obj)
        {
            if (obj is ArtistGroup artistGrp)
            {
                _playerViewModel.PlayCategory(artistGrp.Mp3Files);
            }
        }
    }
}
