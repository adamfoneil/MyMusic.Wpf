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
            var fileInfos = Directory.GetFiles(folder).Select(fileName => new FileInfo(fileName));
            return Scan(fileInfos);
        }

        public static IEnumerable<Mp3File> Scan(IEnumerable<FileInfo> files)
        {
            return files.Select(fi =>
            {
                using (var mp3 = new Mp3(fi.FullName, Mp3Permissions.Read))
                {
                    var result = new Mp3File() { Filename = fi.Name, FullPath = fi.FullName };

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
            });
        }

    }
}
