using System.Collections.Generic;

namespace MyMusic.Wpf.Models
{
    public class ArtistGroup
    {
        public string ArtistName { get; set; }

        public IEnumerable<Mp3File> Mp3Files { get; set; }
    }
}
