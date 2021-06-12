using MyMusic.Wpf.Models;
using MyMusic.Wpf.Services;
using System.Windows;

namespace MyMusic.Wpf
{
    public partial class MainWindow : Window
    {
        private readonly Mp3Ingester _ingester;

        public MainWindow(Mp3Ingester ingester)
        {
            _ingester = ingester;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var model = new Mp3View();
            model.Files = _ingester.GetFiles();
            DataContext = model;
        }
    }
}
