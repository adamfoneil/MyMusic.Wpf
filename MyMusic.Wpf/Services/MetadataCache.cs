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
        private const string _tags = "tags.json";

        private readonly PlayHistory _history;

        public MetadataCache(string rootPath, PlayHistory history)
        {
            _history = history;
            RootPath = rootPath;
        }

        /// <summary>
        /// Occurs when the meta data cache is refreshed.
        /// </summary>
        public event Action Refreshed;

        public string RootPath { get; private set; }
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

        public async Task<IEnumerable<Mp3File>> GetMp3FilesAsync(IEnumerable<string> fileNames)
        {
            List<Mp3File> results = new List<Mp3File>();

            await Task.Run(() =>
            {
                foreach (var fileName in fileNames)
                {
                    var path = Path.GetDirectoryName(fileName);
                    var cacheFile = Path.Combine(path, _folderCache);
                    var mp3File = File.Exists(cacheFile) ? FromMetadata(cacheFile, fileName) : Mp3Metadata.ScanFile(fileName);
                }
            });

            return results;
        }

        public void SaveTags(string mp3File, string[] tags)
        {
            var tagFile = Path.Combine(Path.GetDirectoryName(mp3File), _tags);

            var tagStore = File.Exists(tagFile) ?
                FileUtil.LoadJson<Dictionary<string, string[]>>(tagFile) :
                new Dictionary<string, string[]>();

            tagStore[Path.GetFileName(mp3File)] = tags;

            FileUtil.SaveJson(tagFile, tagStore);
        }

        /// <summary>
        /// deletes the cache info for the folder containing the specified file,
        /// then raises an event to update the display
        /// </summary>        
        public void Rescan(string fileName)
        {
            var path = Path.GetDirectoryName(fileName);
            var cacheFile = Path.Combine(path, _folderCache);

            if (File.Exists(cacheFile))
            {
                File.Delete(cacheFile);
                var fileInfos = Directory.GetFiles(path, "*.mp3", SearchOption.TopDirectoryOnly).Select(fileName => new FileInfo(fileName));
                GetCachedMetadata(path, fileInfos);
                Refreshed?.Invoke();
            }
        }

        private Mp3File FromMetadata(string meta, string fileName)
        {
            var json = File.ReadAllText(meta);
            var mp3Folder = JsonSerializer.Deserialize<Mp3Folder>(json);
            var files = mp3Folder.Files.ToDictionary(item => item.FullPath);
            return files.ContainsKey(fileName) ? files[fileName] : Mp3Metadata.ScanFile(fileName);
        }

        private IEnumerable<Mp3File> GetCachedMetadata(string path, IEnumerable<FileInfo> fileInfos)
        {
            var cacheFile = Path.Combine(path, _folderCache);

            var hashInput = string.Join("\r\n", fileInfos.Select(fi => $"{fi.Name}:{fi.LastWriteTimeUtc}:{fi.CreationTimeUtc}:{fi.Length}"));
            var hash = HashHelper.Md5(hashInput);

            var tags = GetTags(path);

            if (File.Exists(cacheFile))
            {
                var json = File.ReadAllText(cacheFile);
                var mp3Folder = JsonSerializer.Deserialize<Mp3Folder>(json);
                if (!hash.Equals(mp3Folder.Hash))
                {
                    UpdateMetadata(cacheFile, fileInfos, hash, mp3Folder);
                }

                SetTagsAndHistory(mp3Folder.Files, tags);
                return mp3Folder.Files;
            }

            var folder = new Mp3Folder();
            UpdateMetadata(cacheFile, fileInfos, hash, folder);
            SetTagsAndHistory(folder.Files, tags);
            return folder.Files;
        }

        private void SetTagsAndHistory(IEnumerable<Mp3File> files, ILookup<string, string> tags)
        {
            foreach (var file in files)
            {
                if (tags.Contains(file.Filename)) file.Tags = tags[file.Filename].ToArray();
                if (_history.Dictionary.ContainsKey(file.FullPath)) file.LastPlayed = _history.Dictionary[file.FullPath];
            }
        }

        private ILookup<string, string> GetTags(string path)
        {
            List<(string fileName, string tag)> results = new List<(string fileName, string tag)>();

            var tagFile = Path.Combine(path, _tags);
            if (File.Exists(tagFile))
            {
                var tagStore = FileUtil.LoadJson<Dictionary<string, string[]>>(tagFile);
                foreach (var keyPair in tagStore)
                {
                    foreach (var tag in keyPair.Value) results.Add((keyPair.Key, tag));
                }
            }

            return results.ToLookup(item => item.fileName, item => item.tag);
        }

        private void UpdateMetadata(string cacheFile, IEnumerable<FileInfo> files, string hash, Mp3Folder mp3Folder)
        {
            mp3Folder.Files = Mp3Metadata.Scan(files);
            mp3Folder.Hash = hash;
            string json = JsonSerializer.Serialize(mp3Folder, new JsonSerializerOptions()
            {
                WriteIndented = true
            });
            File.WriteAllText(cacheFile, json);
        }

        public void ChangeRootPath(string path)
        {
            RootPath = path;
        }
    }
}
