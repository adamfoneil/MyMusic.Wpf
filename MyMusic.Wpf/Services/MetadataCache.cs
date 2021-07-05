using Id3;
using MyMusic.Wpf.Models;
using MyMusic.Wpf.Static;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyMusic.Wpf.Services
{
    public class MetadataCache
    {
        private const string _folderCache = "meta.json";        

        public MetadataCache(string rootPath)
        {
            RootPath = rootPath;
        }

        public string RootPath { get; }
        public TimeSpan ScanTime { get; private set; }

        public async Task<IEnumerable<Mp3File>> GetMp3FilesAsync(bool rebuild = false)
        {
            List<Mp3File> results = new List<Mp3File>();

            var sw = Stopwatch.StartNew();

            await Task.Run(() =>
            {
                FileScan.Execute(RootPath, "*.mp3", starting: (path, files) =>
                {
                    if (files.Any())
                    {
                        if (rebuild) File.Delete(Path.Combine(path, _folderCache));
                        var fileInfos = files.Select(fileName => new FileInfo(fileName));
                        var data = GetCachedMetadata(path, fileInfos);
                        results.AddRange(data);
                    }
                });
            });

            sw.Stop();
            ScanTime = sw.Elapsed;

            return results;
        }

        private IEnumerable<Mp3File> GetCachedMetadata(string path, IEnumerable<FileInfo> files)
        {
            var cacheFile = Path.Combine(path, _folderCache);

            var hashInput = string.Join("\r\n", files.Select(fi => $"{fi.Name}:{fi.LastWriteTimeUtc}:{fi.Length}"));
            var hash = HashHelper.Md5(hashInput);

            var cachePath = Path.Combine(path, _folderCache);
            if (File.Exists(cachePath))
            {
                var json = File.ReadAllText(cacheFile);
                var mp3Folder = JsonSerializer.Deserialize<Mp3Folder>(json);
                if (!hash.Equals(mp3Folder.Hash))
                {
                    UpdateMetadata(cachePath, files, hash, mp3Folder);
                }

                return mp3Folder.Files;
            }

            var folder = new Mp3Folder();
            UpdateMetadata(cachePath, files, hash, folder);
            return folder.Files;
        }

        private void UpdateMetadata(string cacheFile, IEnumerable<FileInfo> files, string hash, Mp3Folder mp3Folder)
        {
            mp3Folder.Files = ScanMetadata(files);
            mp3Folder.Hash = hash;
            string json = JsonSerializer.Serialize(mp3Folder, new JsonSerializerOptions()
            {
                WriteIndented = true
            });
            File.WriteAllText(cacheFile, json);
        }

        private IEnumerable<Mp3File> ScanMetadata(IEnumerable<FileInfo> files)
        {
            return files.Select(fi =>
            {
                using (var mp3 = new Mp3(fi.FullName, Mp3Permissions.Read))
                {
                    var result = new Mp3File() { Filename = fi.Name, FullPath = fi.FullName };

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
