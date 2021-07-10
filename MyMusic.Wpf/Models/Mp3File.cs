using MyMusic.Wpf.Static;
using System.IO;

namespace MyMusic.Wpf.Models
{
    public class Mp3File
    {
        public string FullPath { get; set; }
        public string Filename { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Title { get; set; }
        public int? TrackNumber { get; set; }
        public int? TrackCount { get; set; }

        public string DisplayTitle =>
            !string.IsNullOrEmpty(Title) ? Title :
            Path.GetFileName(Filename);

        public string DisplayArtist =>
            !string.IsNullOrEmpty(Artist) ? Artist :
            FileUtil.FolderName(Filename, 2);

        public string DisplayAlbum =>
            !string.IsNullOrEmpty(Album) ? Album :
            FileUtil.FolderName(Filename, 1);
    }
}
