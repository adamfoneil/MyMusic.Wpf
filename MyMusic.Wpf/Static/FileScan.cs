using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyMusic.Wpf.Static
{
    public static class FileScan
    {
        public static void Execute(string path, string searchPattern, Action<string, IEnumerable<string>> starting = null, Action<string, IEnumerable<string>> ending = null)
        {
            var files = GetFiles(path, searchPattern);

            starting?.Invoke(path, files);

            var dirs = GetDirectories(path);

            foreach (var dir in dirs)
            {
                Execute(dir, searchPattern, starting, ending);
            }

            ending?.Invoke(path, files);
        }

        private static IEnumerable<string> GetFiles(string path, string searchPattern)
        {
            try
            {
                return Directory.GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
            }
            catch
            {
                return Enumerable.Empty<string>();
            }
        }

        private static IEnumerable<string> GetDirectories(string path)
        {
            try
            {
                return Directory.GetDirectories(path);
            }
            catch
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}
