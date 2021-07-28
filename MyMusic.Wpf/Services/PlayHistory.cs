using MyMusic.Wpf.Static;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace MyMusic.Wpf.Services
{
    public class PlayHistory
    {
        private readonly string _rootPath;
        private readonly string _fileName;
        private readonly int _maxEntries;

        private Dictionary<string, DateTime> _history;

        public PlayHistory(string rootPath, int maxEntries)
        {
            _rootPath = rootPath;
            _maxEntries = maxEntries;
            _fileName = Path.Combine(_rootPath, "history.json");
            _history = File.Exists(_fileName) ? FileUtil.LoadJson<Dictionary<string, DateTime>>(_fileName) : new Dictionary<string, DateTime>();
        }

        public void Add(string path)
        {
            _history[path] = DateTime.Now;

            if (_history.Count > _maxEntries)
            {
                var keepEntries = QueryOrdered();
                _history = keepEntries.ToDictionary(kp => kp.Key, kp => kp.Value);
            }

            var json = JsonSerializer.Serialize(_history, new JsonSerializerOptions()
            {
                WriteIndented = true
            });

            File.WriteAllText(_fileName, json);
        }

        public IEnumerable<KeyValuePair<string, DateTime>> QueryOrdered() => _history.OrderByDescending(kp => kp.Value).Take(_maxEntries);

        public Dictionary<string, DateTime> Dictionary => _history;
    }
}
