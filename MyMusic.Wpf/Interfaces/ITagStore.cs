using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.Wpf.Interfaces
{
    public interface ITagStore
    {
        /// <summary>
        /// apply a set of tags to a set of files
        /// </summary>
        Task SaveAsync(IEnumerable<string> tags, IEnumerable<string> files);

        /// <summary>
        /// splits tag input into separate tags using a mix of commas, semicolons or spaces and quoted items
        /// </summary>
        IEnumerable<string> Parse(string tagExpression);

        /// <summary>
        /// query the tags for a set of files (intended to work with virtualized list of files in a grid),
        /// i.e. for the files I can currently see in the grid (not necessarily the entire file set).
        /// Keyed to filename
        /// </summary>
        Task<ILookup<string, string>> LoadAsync(IEnumerable<string> files);
    }
}
