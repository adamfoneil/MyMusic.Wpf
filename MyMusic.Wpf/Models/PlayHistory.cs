using MyMusic.Wpf.Static;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MyMusic.Wpf.Models
{
    public class PlayHistory : IDisposable
    {
        private readonly string _rootPath;
        private readonly string _fileName;        
        private readonly Dictionary<string, DateTime> _history;

        public PlayHistory(string rootPath)
        {
            _rootPath = rootPath;
            _fileName = Path.Combine(_rootPath, "history.json");
            _history = File.Exists(_fileName) ? FileUtil.LoadJson<Dictionary<string, DateTime>>(_fileName) : new Dictionary<string, DateTime>();
        }

        public void Add(string mp3File)
        {
            _history[mp3File] = DateTime.Now;
        }

        public void Dispose()
        {
            var json = JsonSerializer.Serialize(_history);
            File.WriteAllText(_fileName, json);
        }
    }
}
