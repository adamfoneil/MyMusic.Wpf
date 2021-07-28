using Microsoft.Extensions.DependencyInjection;
using MyMusic.Wpf.Controls;
using MyMusic.Wpf.Models;
using MyMusic.Wpf.Services;
using MyMusic.Wpf.ViewModels;
using System.Windows;

namespace MyMusic.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            var settings = Settings.Load();
            settings.RootPathChanged += Settings_RootPathChanged;
            services.AddSingleton(settings);
            var history = new PlayHistory(rootPath: settings.RootPath, maxEntries: 100);
            var cache = new MetadataCache(settings.RootPath, history);
            services.AddSingleton(history);
            services.AddSingleton(cache);
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<Mp3View>();
            services.AddSingleton<PlayerViewModel>();
            services.AddSingleton<SettingsViewModel>();
        }

        private void Settings_RootPathChanged(string obj)
        {
            var metadataCache = _serviceProvider.GetService<MetadataCache>();
            metadataCache.ChangeRootPath(obj);
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
