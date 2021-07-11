using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMusic.Wpf.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    [TestClass]
    public class TagTesting
    {
        [TestMethod]
        public void TagParseSimpleCase()
        {
            var tagStore = new TagStore();
            var tags = tagStore.Parse("this    that  other");
            Assert.IsTrue(tags.SequenceEqual(new string[] { "this", "that", "other" }));
        }

        [TestMethod]
        public void TagParseMixedDelimiters()
        {
            var tagStore = new TagStore();
            var tags = tagStore.Parse("hello goodbye; forever, \"and then\"");
            Assert.IsTrue(tags.SequenceEqual(new string[] { "and then", "hello", "goodbye", "forever" }));
        }
    }
}
