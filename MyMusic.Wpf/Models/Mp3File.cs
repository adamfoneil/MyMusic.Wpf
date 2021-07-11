using MyMusic.Wpf.Static;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyMusic.Wpf.Models
{
    public class Mp3File
    {
        public string FullPath { get; set; }
        public string Filename { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Title { get; set; }
        public int? TrackNumber { get; set; }
        public int? TrackCount { get; set; }

        public string DisplayTitle =>
            !string.IsNullOrWhiteSpace(Title) ? Title :
            Path.GetFileName(Filename);

        public string DisplayArtist =>
            !string.IsNullOrWhiteSpace(Artist) ? Artist :
            FileUtil.FolderName(Filename, 2);

        public string DisplayAlbum =>
            !string.IsNullOrWhiteSpace(Album) ? Album :
            FileUtil.FolderName(Filename, 1);

        public bool IsSearchHit(string query) => 
            (IsTargetedSearch("artist:", query, DisplayArtist)) ? true :
            (IsTargetedSearch("album:", query, DisplayAlbum)) ? true :
            SearchValues.ContainsAny(query);

        private IEnumerable<string> SearchValues => new[]
        {
            Filename,
            DisplayArtist,
            DisplayAlbum,
            Title
        }.Where(val => !string.IsNullOrWhiteSpace(val)).Select(val => val.ToLower());

        private bool IsTargetedSearch(string token, string query, string stringValue)
        {
            if (query.StartsWith(token) && query.Length > token.Length)
            {
                return stringValue?.ToLower().StartsWith(query.Substring(token.Length)) ?? false;
            }

            return false;
        }
    }
}
