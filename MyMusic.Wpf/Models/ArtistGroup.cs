using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Wpf.Models
{
    public class ArtistGroup 
    {
        public string ArtistName { get; set; }

        public IEnumerable<Mp3File> Mp3Files { get; set; }
    }
}
