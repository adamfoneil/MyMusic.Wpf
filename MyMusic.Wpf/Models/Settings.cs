using MyMusic.Wpf.Static;
using System;
using System.IO;

namespace MyMusic.Wpf.Models
{
    public enum AllFilesView
    {
        Flat,
        ByArtist,
        ByAlbum
    }

    public class Settings
    {
        private string _rootPath;

        /// <summary>
        /// Occurs when [root path changed].
        /// </summary>
        public event Action<string> RootPathChanged;

        /// <summary>
        /// all your MP3 files are under this path
        /// </summary>
        public string RootPath
        {
            get => _rootPath;
            set
            {
                _rootPath = value;
                RootPathChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// default view of All Files tab
        /// </summary>
        public AllFilesView View { get; set; }

        public static string Filename => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyMusic.Wpf", "settings.json");

        public static Settings Load() => File.Exists(Filename) ?
            FileUtil.LoadJson<Settings>(Filename) :
            new Settings() { RootPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), View = AllFilesView.Flat };

    }
}
