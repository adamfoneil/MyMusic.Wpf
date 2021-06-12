using System;

namespace MyMusic.Wpf.Models
{
    public class Mp3File
    {
        public string Path { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Title { get; set; }
        public int? TrackNumber { get; set; }
        public int? TrackCount { get; set; }

        public static Mp3File FromFilename(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
