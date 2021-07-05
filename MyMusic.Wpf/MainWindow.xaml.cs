using MyMusic.Wpf.Models;
using MyMusic.Wpf.Services;
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
            DataContext = model;
        }

        private void dgFiles_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var grid = sender as DataGrid;
            var mp3file = grid.CurrentItem as Mp3File;
            player.PlayNow(mp3file);
        }

        private async void btnRebuild_Click(object sender, RoutedEventArgs e)
        {
            var model = this.DataContext as Mp3View;
            model.AllFiles = await _cache.GetMp3FilesAsync(rebuild: true);
            model.LoadTime = _cache.ScanTime;
        }
    }
}
