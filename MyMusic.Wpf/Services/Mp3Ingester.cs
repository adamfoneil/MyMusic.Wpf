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
                    var tag = mp3.GetTag(Id3TagFamily.Version2X);
                    var result = new Mp3File()
                    {
                        Path = fileName,
                        Artist = tag.Artists,
                        Album = tag.Album,
                        Title = tag.Title
                    };

                    if (tag.Track.IsAssigned)
                    {
                        result.TrackNumber = tag.Track.Value;
                        result.TrackCount = tag.Track.TrackCount;
                    }

                    return result;
                }
            });
        }
    }
}
