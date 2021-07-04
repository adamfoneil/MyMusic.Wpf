using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMusic.Wpf.Services;
using System.Threading.Tasks;

namespace Testing
{
    [TestClass]
    public class CacheTests
    {
        [TestMethod]
        public async Task BuildCache()
        {
            var cache = new MetadataCache(@"C:\Users\adamo\OneDrive\Music");
            var files = await cache.GetMp3FilesAsync();
        }
    }
}
