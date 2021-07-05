using System;
using System.Collections.Generic;

namespace MyMusic.Wpf.Models
{
    public class Mp3View
    {
        public IEnumerable<Mp3File> AllFiles { get; set; }
        public TimeSpan LoadTime { get; set; }
    }
}
