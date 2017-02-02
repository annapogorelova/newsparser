using System.Linq;
using Microsoft.EntityFrameworkCore;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.NewsSources
{
    public class NewsSourceRepository: INewsSourceRepository
    {
        private readonly AppDbContext _dbContext;

        public NewsSourceRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<NewsSource> GetNewsSources()
        {
            return _dbContext.NewsSources;
        }

        public NewsSource GetNewsSourceById(int id)
        {
            return _dbContext.NewsSources.Find(id);
        }

        public void AddNewsSource(NewsSource newsSource)
        {
            _dbContext.NewsSources.Add(newsSource);
            _dbContext.SaveChanges();
        }

        public void UpdateNewsSource(NewsSource newsSource)
        {
            _dbContext.Entry(newsSource).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void DeleteNewsSource(int id)
        {
            var newsSourceToDelete = _dbContext.NewsSources.Find(id);
            if (newsSourceToDelete != null)
            {
                _dbContext.NewsSources.Remove(newsSourceToDelete);
                _dbContext.SaveChanges();
            }
        }
    }
}
