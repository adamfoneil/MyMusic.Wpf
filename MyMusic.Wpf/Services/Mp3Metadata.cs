using Id3;
using MyMusic.Wpf.Models;
using MyMusic.Wpf.Static;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static Mp3File ScanFile(string fileName, bool debug = false)
        {
            if (debug && Debugger.IsAttached) Debugger.Break();

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

                // assume some values based on the path if they can't be read from .mp3 directly
                if (string.IsNullOrEmpty(result.Title)) result.Title = Path.GetFileName(fileName);
                if (string.IsNullOrEmpty(result.Artist)) result.Artist = FileUtil.FolderName(fileName, 3);
                if (string.IsNullOrEmpty(result.Album)) result.Album = FileUtil.FolderName(fileName, 2);

                return result;
            }
        }

        public static IEnumerable<Mp3File> Scan(IEnumerable<FileInfo> files) => files.Select(fi =>
        {
            var result = ScanFile(fi.FullName);
            result.DateAdded = fi.CreationTimeUtc;
            return result;
        });
    }
}
