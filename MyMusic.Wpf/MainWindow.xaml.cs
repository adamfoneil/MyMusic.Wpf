using MyMusic.Wpf.Models;
using MyMusic.Wpf.Services;
using Prism.Commands;
using System.Windows;
using System.Windows.Controls;

namespace MyMusic.Wpf
{
    public partial class MainWindow : Window
    {
        private readonly MetadataCache _cache;

        public MainWindow(MetadataCache cache)
        {
            _cache = cache;
            InitializeComponent();            
        }

        public MetadataCache Cache => _cache;

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var model = new Mp3View();
            model.AllFiles = await _cache.GetMp3FilesAsync();
            model.LoadTime = _cache.ScanTime;
            model.PlayNextCommand = new DelegateCommand<Mp3File>(file => player.PlayNext(file));
            model.PlayAtEndCommand = new DelegateCommand<Mp3File>(file => player.PlayAtEnd(file));
            this.DataContext = model;
        }

        private async void btnRebuild_Click(object sender, RoutedEventArgs e)
        {
            var model = this.DataContext as Mp3View;
            model.AllFiles = await _cache.GetMp3FilesAsync(rebuild: true);
            model.LoadTime = _cache.ScanTime;
        }

        /// <summary>
        /// how to get rid of this?
        /// </summary>        
        private void dgFiles_Mp3FileSelected(Mp3File mp3file)
        {
            player.PlayNow(mp3file);
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            // search behavior would be here
        }
    }
}
