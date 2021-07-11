using MyMusic.Wpf.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyMusic.Wpf.Services
{
    public class TagStore : ITagStore
    {
        public Task<ILookup<string, string>> LoadAsync(IEnumerable<string> files)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> Parse(string tagExpression)
        {
            // https://stackoverflow.com/a/171499/2023653 comment from Aran-Fey
            var quoted = Regex.Matches(tagExpression, @"([""'])(?:\\.|[^\\])*?\1")
                .OfType<Match>()
                .Select(m => m.Value.Substring(1, m.Value.Length - 2)) // strip enclosing quotes
                .ToArray();

            foreach (var item in quoted) yield return item;
            
            quoted.ToList().ForEach(val => tagExpression = tagExpression.Replace("\"" + val + "\"", string.Empty));

            var split = tagExpression.Split(new char[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in split) yield return item.Trim();
        }

        public Task SaveAsync(IEnumerable<string> tags, IEnumerable<string> files)
        {
            throw new NotImplementedException();
        }
    }
}
