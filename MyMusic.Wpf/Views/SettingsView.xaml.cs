using MyMusic.Wpf.ViewModels;
using System.Windows;

namespace MyMusic.Wpf.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Window
    {
        public SettingsView(SettingsViewModel settingsVM)
        {
            InitializeComponent();
            DataContext = settingsVM;
        }
    }
}
