using System.Linq;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.NewsSources
{
    public interface INewsSourceRepository
    {
        IQueryable<NewsSource> GetNewsSources();
        NewsSource GetNewsSourceById(int id);
        void AddNewsSource(NewsSource newsSource);
        void UpdateNewsSource(NewsSource newsSource);
        void DeleteNewsSource(int id);
    }
}
