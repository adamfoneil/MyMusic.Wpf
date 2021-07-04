namespace MyMusic.Wpf.Static
{
    public static class PathUtil
    {
        public static string Folder(string path, int segmentFromRight)
        {
            var parts = path.Split('\\');
            return (parts.Length >= segmentFromRight) ? parts[^segmentFromRight] : path;
        }
    }
}
