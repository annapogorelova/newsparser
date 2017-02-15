using System.Threading.Tasks;
using NewsParser.DAL.Models;

namespace NewsParser.FeedParser
{
    public interface IFeedParser
    {
        Task ParseNewsSource(NewsSource newsSource);
    }
}
