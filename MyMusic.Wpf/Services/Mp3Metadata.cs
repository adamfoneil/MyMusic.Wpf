using Id3;
using MyMusic.Wpf.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyMusic.Wpf.Services
{
    public static class Mp3Metadata
    {
        public static IEnumerable<Mp3File> Scan(string folder)
        {
            return Directory.GetFiles(folder).Select(fileName => ScanFile(fileName));            
        }

        public static Mp3File ScanFile(string fileName)
        {
            using (var mp3 = new Mp3(fileName, Mp3Permissions.Read))
            {
                var result = new Mp3File() { Filename = Path.GetFileName(fileName), FullPath = fileName };

                var tag = mp3.GetTag(Id3TagFamily.Version2X) ?? mp3.GetTag(Id3TagFamily.Version1X);
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
        }

        public static IEnumerable<Mp3File> Scan(IEnumerable<string> files) => files.Select(path => ScanFile(path));

        public static IEnumerable<Mp3File> Scan(IEnumerable<FileInfo> files) => files.Select(fi => ScanFile(fi.FullName));
    }
}
