using MyMusic.Wpf.Models;
using MyMusic.Wpf.Services;
using System.Windows;

namespace MyMusic.Wpf
{
    public partial class MainWindow : Window
    {
        private readonly MetadataCache _ingester;

        public MainWindow(MetadataCache ingester)
        {
            _ingester = ingester;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var model = new Mp3View();
            //model.Files = _ingester.GetFiles();
            DataContext = model;
            dgFiles.ItemsSource = ((Mp3View)DataContext).Files;
        }
    }
}
