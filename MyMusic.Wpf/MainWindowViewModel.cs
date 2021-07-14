using MyMusic.Wpf.Controls;
using MyMusic.Wpf.Models;
using MyMusic.Wpf.Services;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

namespace MyMusic.Wpf
{
    public class MainWindowViewModel : BindableBase
    {
        private MetadataCache _cache;
        private PlayerViewModel _playerViewModel;
        public MainWindowViewModel(MetadataCache cache, PlayerViewModel playerViewModel, Mp3View mp3View)
        {
            _cache = cache; ;
            _playerViewModel = playerViewModel;
            Mp3View = mp3View;
            RebuildCommand = new DelegateCommand(Rebuild);
            PlayNextCommand = new DelegateCommand<Mp3File>(mp3 => PlayNext(mp3));
            PlayAtEndCommand = new DelegateCommand<Mp3File>(mp3 => PlayAtEnd(mp3));
        }

        public Mp3View Mp3View { get; set; }

        public ICommand RebuildCommand { get; set; }

        public ICommand PlayAtEndCommand { get; set; }

        public ICommand PlayNextCommand { get; set; }

        public PlayerViewModel PlayerViewModel
        {
            get => _playerViewModel;
            set
            {
                SetProperty(ref _playerViewModel, value);
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
    }
}
