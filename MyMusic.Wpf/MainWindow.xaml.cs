using MyMusic.Wpf.Models;
using MyMusic.Wpf.Services;
using System.Threading.Tasks;
using System.Windows;

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
            model.Files = await _cache.GetMp3FilesAsync();
            model.LoadTime = _cache.ScanTime;
            DataContext = model;
            dgFiles.ItemsSource = ((Mp3View)DataContext).Files;
        }
    }
}
