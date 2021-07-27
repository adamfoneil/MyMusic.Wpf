using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace MyMusic.Wpf.Static
{
    public static class FileUtil
    {
        public static string FolderName(string path, int segmentFromRight)
        {
            var parts = path.Split('\\');
            return (parts.Length >= segmentFromRight) ? parts[^segmentFromRight] : path;
        }

        public static T LoadJson<T>(string fileName)
        {
            var json = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<T>(json);
        }

        public static void RevealInExplorer(string fileName)
        {
            ProcessStartInfo psi = new ProcessStartInfo("explorer.exe");
            psi.Arguments = $"/select,\"{fileName}\"";
            Process.Start(psi);
        }

        public static void SaveJson<T>(string fileName, T data, JsonSerializerOptions options = null)
        {
            var json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(fileName, json);
        }
    }
}
