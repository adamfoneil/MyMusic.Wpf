using MyMusic.Wpf.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

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
        public DateTime? DateAdded { get; set; }

        /// <summary>
        /// based on PlayHistory service, so we persist this elsewhere
        /// </summary>
        [JsonIgnore]
        public DateTime? LastPlayed { get; set; }

        /// <summary>
        /// this gets serialized in a special way so that data doesn't get lost when cache is deleted
        /// </summary>
        [JsonIgnore]
        public string[] Tags { get; set; }

        public bool IsSearchHit(string query) =>
            IsTargetedSearch(query, new[]
            {
                ("artist:", Artist),
                ("album:", Album),
                ("title:", Title)
            }) ? true : SearchValues.ContainsAny(query);

        private IEnumerable<string> SearchValues => new[]
        {
            Filename,
            Artist,
            Album,
            Title
        }.Where(val => !string.IsNullOrWhiteSpace(val)).Select(val => val.ToLower());

        private bool IsTargetedSearch(string query, IEnumerable<(string token, string value)> options)
        {
            foreach (var item in options)
            {
                if (query.StartsWith(item.token) && query.Length > item.token.Length)
                {
                    return item.value?.ToLower().StartsWith(query.Substring(item.token.Length).Trim()) ?? false;
                }
            }

            return false;
        }
    }
}
