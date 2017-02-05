using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Xml.Linq;
using NewsParser.DAL.Models;
using NewsParser.DAL.News;
using System.Linq;
using System.Threading.Tasks;

namespace NewsParser.Parser
{
    /// <summary>
    /// Class contains methods for parsing the news from the specified sources
    /// </summary>
    public class FeedParser : IFeedParser
    {
        private readonly INewsRepository _newsRepository;

        public FeedParser(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task Parse(NewsSource newsSource)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string result = await httpClient.GetStringAsync(newsSource.MainUrl);

                XElement xmlItems = XElement.Parse(result);
                List<XElement> elements = xmlItems.Descendants("item").ToList();

                var existingSourceNews = _newsRepository.GetNewsBySource(newsSource.Id);

                foreach (var rssItem in elements)
                {
                    Console.WriteLine(newsSource.Name);
                    var newsItem = new NewsItem()
                    {
                        SourceId = newsSource.Id,
                        Title = rssItem.Element("title").Value,
                        Description = rssItem.Element("description").Value,
                        DateAdded = DateTime.Parse(rssItem.Element("pubDate").Value),
                        CategoryId = 10,
                        LinkToSource = rssItem.Element("link").Value
                    };

                    if (!existingSourceNews.Any(n => n.LinkToSource == newsItem.LinkToSource))
                    {
                        _newsRepository.AddNewsItem(newsItem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }   
        }
    }
}
