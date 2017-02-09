using System.Collections.Generic;
using System.Linq;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.NewsTags
{
    public interface INewsTagRepository
    {
        IQueryable<NewsTag> GetNewsTags();
        NewsTag GetNewsTagById(int id);
        NewsTag GetNewsTagByName(string name);
        NewsTag AddNewsTag(NewsTag newsTag);
        void AddNewsTags(IEnumerable<NewsTag> newsTags);
    }
}
