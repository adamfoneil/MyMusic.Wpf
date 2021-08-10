using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMusic.Wpf.Models;
using MyMusic.Wpf.Services;
using System.Linq;

namespace Testing
{
    [TestClass]
    public class SortingTests
    {
        [TestMethod]
        public void FavoriteArtists()
        {
            var cache = new MetadataCache(@"C:\Users\adamo\OneDrive\Music", null);
            var files = cache.GetMp3FilesAsync().Result;

            var favoriteArtists = Mp3View.SortRules[SortOptions.MostRepresented].Invoke(files).Take(30).ToArray();
            Assert.IsTrue(favoriteArtists.All(file => file.Artist.Equals("Bob Dylan; The Band")));
        }
    }
}
