using System;
using System.Collections.Generic;
using System.Linq;

namespace MyMusic.Wpf.Static
{
    public static class SearchHelper
    {
        public static bool ContainsAny<T>(this IEnumerable<T> enumerable, string search, Func<T, string> stringAccessor) =>
            enumerable.Any(item => stringAccessor.Invoke(item).Contains(search));

        public static bool ContainsAny(this IEnumerable<string> enumerable, string search) =>
            ContainsAny(enumerable, search, item => item.ToLower());
    }
}
