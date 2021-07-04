using System.Collections.Generic;

namespace MyMusic.Wpf.Models
{
    public class Mp3Folder
    {
        public IEnumerable<Mp3File> Files { get; set; }
        public string Hash { get; set; }
        public string Thumbnail { get; set; }
    }
}
