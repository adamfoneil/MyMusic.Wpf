using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMusic.Wpf.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Testing
{
    [TestClass]
    public class CacheTests
    {
        [TestMethod]
        public async Task BuildCache()
        {
            var cache = new MetadataCache(@"C:\Users\adamo\OneDrive\Music", null);
            var files = await cache.GetMp3FilesAsync();
        }

        [TestMethod]
        public void Mp3MetadataScan()
        {
            var result = Mp3Metadata.Scan(@"C:\Users\adamo\OneDrive\Music\Sonic Mayhem\Quake II Soundtrack").ToArray();
        }
    }
}
