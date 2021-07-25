using MyMusic.Wpf.Services;
using MyMusic.Wpf.Static;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyMusic.Wpf.Models
{
    public class PlayHistory
    {
        private readonly string _rootPath;
        private readonly string _fileName;
        private readonly int _maxEntries;
        private readonly MetadataCache _cache;

        private Dictionary<string, DateTime> _history;

        public PlayHistory(string rootPath, int maxEntries, MetadataCache cache)
        {
            _rootPath = rootPath;
            _maxEntries = maxEntries;
            _fileName = Path.Combine(_rootPath, "history.json");
            _history = File.Exists(_fileName) ? FileUtil.LoadJson<Dictionary<string, DateTime>>(_fileName) : new Dictionary<string, DateTime>();
            _cache = cache;
        }

        public void Add(string path)
        {
            _history[path] = DateTime.Now;

            if (_history.Count > _maxEntries)
            {
                var keepEntries = QueryInternal();
                _history = keepEntries.ToDictionary(kp => kp.Key, kp => kp.Value);
            }

            var json = JsonSerializer.Serialize(_history, new JsonSerializerOptions()
            {
                WriteIndented = true
            });

            File.WriteAllText(_fileName, json);
        }

        private IEnumerable<KeyValuePair<string, DateTime>> QueryInternal() => _history.OrderByDescending(kp => kp.Value).Take(_maxEntries);

        public async Task<IEnumerable<Mp3File>> QueryAsync()
        {
            var fileNames = QueryInternal().Select(kp => kp.Key);
            return await _cache.GetMp3FilesAsync(fileNames);
        }
    }
}
