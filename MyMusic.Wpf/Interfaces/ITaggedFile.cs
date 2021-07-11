namespace MyMusic.Wpf.Interfaces
{
    public interface ITaggedFile
    {
        string FullPath { get; }
        string[] Tags { get; }
    }
}
