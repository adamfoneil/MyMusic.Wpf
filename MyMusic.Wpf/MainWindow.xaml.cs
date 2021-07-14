using MyMusic.Wpf.Models;
using MyMusic.Wpf.Services;
using Prism.Commands;
using System.Windows;

namespace MyMusic.Wpf
{
    public partial class MainWindow : Window
    {        
        private readonly Settings _settings;
        private readonly MainWindowViewModel _mainWindowViewModel;

        public MainWindow(Settings settings, MainWindowViewModel mainWindowViewModel)
        {
            _settings = settings;
            InitializeComponent();
            _mainWindowViewModel = mainWindowViewModel;
            DataContext = mainWindowViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = "My Music - " + _settings.RootPath;
            _mainWindowViewModel.LoadFiles();
        }

        /// <summary>
        /// how to get rid of this?
        /// </summary>        
        private void dgFiles_Mp3FileSelected(Mp3File mp3file)
        {
            if(mp3file != null)
            {
                _mainWindowViewModel.PlayerViewModel.PlayNow(mp3file);
            }            
        }
    }
}
