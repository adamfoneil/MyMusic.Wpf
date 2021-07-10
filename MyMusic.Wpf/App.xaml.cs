using Microsoft.Extensions.DependencyInjection;
using MyMusic.Wpf.Models;
using MyMusic.Wpf.Services;
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

            services.AddSingleton(settings);
            services.AddSingleton(new PlayHistory(settings.RootPath));
            services.AddSingleton(new MetadataCache(settings.RootPath));
            services.AddSingleton<MainWindow>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
