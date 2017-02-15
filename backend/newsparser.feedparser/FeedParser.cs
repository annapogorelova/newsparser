using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Xml.Linq;
using NewsParser.DAL.Models;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NewsParser.BL.Services.News;
using NewsParser.BL.Services.NewsSources;

namespace NewsParser.FeedParser
{
    /// <summary>
    /// Class contains methods for parsing the news from the specified sources
    /// </summary>
    public class FeedParser : IFeedParser
    {
        private readonly INewsBusinessService _newsBusinessService;
        private readonly INewsSourceBusinessService _newsSourceBusinessService;

        public FeedParser(INewsBusinessService newsBusinessService, INewsSourceBusinessService newsSourceBusinessService)
        {
            _newsBusinessService = newsBusinessService;
            _newsSourceBusinessService = newsSourceBusinessService;
        }

        /// <summary>
        /// Parses news source's RSS and saves new news into database
        /// </summary>
        /// <param name="newsSource">NewsSource object</param>
        /// <returns>Async Task object</returns>
        public async Task ParseNewsSource(NewsSource newsSource)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string result = await httpClient.GetStringAsync(newsSource.RssUrl);

                XElement xmlItems = XElement.Parse(result);
                List<XElement> xmlElements = xmlItems.Descendants("item").ToList();
                ParseNewsSourceRss(xmlElements, newsSource);
                newsSource.DateFeedUpdated = DateTime.UtcNow;
                _newsSourceBusinessService.UpdateNewsSource(newsSource);
            }
            catch (Exception e)
            {
                throw new FeedParsingException("Failed to parse RSS feed", e);
            }
        }

        /// <summary>
        /// Parses the incoming news source RSS feed XML
        /// </summary>
        /// <param name="xmlElements">The list of XML elements of RSS feed</param>
        /// <param name="newsSource">NewsSource object</param>
        /// <returns>List of NewsItem</returns>
        private void ParseNewsSourceRss(List<XElement> xmlElements, NewsSource newsSource)
        {
            var addedNewsItems = new List<NewsItem>();

            foreach (var rssItem in xmlElements)
            {
                var rssItemDescription = rssItem.Element("description").Value;
                var tags = ExtractRssItemTags(rssItem);

                var newsItem = new NewsItem
                {
                    SourceId = newsSource.Id,
                    Title = rssItem.Element("title").Value,
                    Description = CleanHtmlString(rssItemDescription),
                    DateAdded = DateTime.Parse(rssItem.Element("pubDate").Value),
                    LinkToSource = rssItem.Element("link").Value,
                    ImageUrl = ExtractFirstImage(rssItemDescription)
                };

                if (_newsBusinessService.GetNewsItemByLink(newsItem.LinkToSource) == null)
                {
                    var addedNewsItem = _newsBusinessService.AddNewsItem(newsItem);
                    addedNewsItems.Add(addedNewsItem);
                    _newsBusinessService.AddTagsToNewsItem(addedNewsItem.Id, tags);
                }
            }
        }

        /// <summary>
        /// Performs the cleaning action for html string (removing some tags, etc.)
        /// </summary>
        /// <param name="html">Html string</param>
        /// <returns>Cleaned html string</returns>
        private string CleanHtmlString(string html)
        {
            string cleanHtmlString = RemoveLineBreakes(html);
            cleanHtmlString = RemoveImages(cleanHtmlString);
            return cleanHtmlString;
        }

        /// <summary>
        /// Removes line breaks in html string
        /// </summary>
        /// <param name="html">Html string</param>
        /// <returns>Cleaned html string</returns>
        private string RemoveLineBreakes(string html)
        {
            var regex = new Regex(@"([\b\s]*<[\b\s]*[b][r][\s]*/?[\b\s]*>)", RegexOptions.IgnoreCase);
            return regex.Replace(html, string.Empty);
        }

        /// <summary>
        /// Removes img tags from html string
        /// </summary>
        /// <param name="html">Html string</param>
        /// <returns>Cleaned html string</returns>
        private string RemoveImages(string html)
        {
            var regex = new Regex(@"<img.+?/?>", RegexOptions.IgnoreCase);
            return regex.Replace(html, string.Empty);
        }

        /// <summary>
        /// Extracts the first img tag's src attribute from html string
        /// </summary>
        /// <param name="html">Html string</param>
        /// <returns>First img tag's src attribute or null if no img tags found</returns>
        private string ExtractFirstImage(string html)
        {
            var match = Regex.Match(html, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value : null;
        }

        /// <summary>
        /// Extracts the 'category' xml nodes from RSS feed item
        /// </summary>
        /// <param name="rssItem">XElement object</param>
        /// <returns>List of categorie's names (strings)</returns>
        private List<string> ExtractRssItemTags(XElement rssItem)
        {
            var categoryElements = rssItem.Elements("category").ToList();
            return categoryElements.Select(e => e.Value.ToLower()).ToList();
        }
    }
}
