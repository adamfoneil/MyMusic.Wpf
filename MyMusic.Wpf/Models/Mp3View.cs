using System;
using System.Collections.Generic;

namespace MyMusic.Wpf.Models
{
    public class Mp3View
    {
        public IEnumerable<Mp3File> Files { get; set; }
        public TimeSpan LoadTime { get; set; }
    }
}
