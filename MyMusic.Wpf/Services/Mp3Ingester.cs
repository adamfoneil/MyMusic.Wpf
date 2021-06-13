using Id3;
using MyMusic.Wpf.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyMusic.Wpf.Services
{
    public class Mp3Ingester
    {
        public Mp3Ingester(string rootPath)
        {
            RootPath = rootPath;
        }

        public string RootPath { get; }

        public IEnumerable<Mp3File> GetFiles()
        {
            var files = Directory.GetFiles(RootPath, "*.mp3", SearchOption.AllDirectories);
            
            return files.Select(fileName =>
            {
                using (var mp3 = new Mp3(fileName, Mp3Permissions.Read))
                {                    
                    var path = fileName.Substring(RootPath.Length);
                    if (path.StartsWith("\\")) path = path.Substring(1);
                    var result = new Mp3File() { Path = path };

                    var tag = mp3.GetTag(Id3TagFamily.Version2X);
                    if (tag != null)
                    {
                        result.Artist = tag.Artists;
                        result.Album = tag.Album;
                        result.Title = tag.Title;

                        if (tag.Track.IsAssigned)
                        {
                            result.TrackNumber = tag.Track.Value;
                            result.TrackCount = tag.Track.TrackCount;
                        }
                    }

                    return result;
                }
            });
        }
    }
}
