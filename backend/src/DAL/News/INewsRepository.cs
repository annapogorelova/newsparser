using System.Linq;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.News
{
    public interface INewsRepository
    {
        IQueryable<NewsItem> GetNews(int startIndex = 0, int numResults = 5, int? categoryId = null, int? sourceId = null);
        IQueryable<NewsItem> GetNewsByCategory(int categoryId);
        IQueryable<NewsItem> GetNewsBySource(int sourceId);
        NewsItem GetNewsById(int id);
    }
}
