using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Media;

namespace Testing
{
    [TestClass]
    public class FileHandling
    {
        [TestMethod]
        public void FileExistsFalseNegative()
        {
            var exists = File.Exists(@"C:\Users\adamo\OneDrive\Music\My Bloody Valentine\EPs 1988-1991 Disc 2\01-10- Good for You [#].mp3");
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void SoundPlayerBehavior()
        {
            var player = new SoundPlayer();
            
        }
    }
}
