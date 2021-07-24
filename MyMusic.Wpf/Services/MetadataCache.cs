﻿using MyMusic.Wpf.Models;
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

        private Mp3File FromMetadata(string meta, string fileName)
        {
            var json = File.ReadAllText(meta);
            var mp3Folder = JsonSerializer.Deserialize<Mp3Folder>(json);
            var files = mp3Folder.Files.ToDictionary(item => item.FullPath);
            return files.ContainsKey(fileName) ? files[fileName] : Mp3Metadata.ScanFile(fileName);
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
