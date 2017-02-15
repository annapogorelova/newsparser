using System.Threading.Tasks;
using NewsParser.DAL.Models;

namespace NewsParser.FeedParser
{
    /// <summary>
    /// Interface contains methods for feed parsing
    /// </summary>
    public interface IFeedParser
    {
        Task ParseNewsSource(NewsSource newsSource);
    }
}
