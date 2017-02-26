using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Xml.Linq;
using NewsParser.DAL.Models;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using newsparser.feedparser;

namespace NewsParser.FeedParser
{
    /// <summary>
    /// Class contains methods for parsing the news from the specified sources
    /// </summary>
    public class RssParser : IFeedParser
    {
        /// <summary>
        /// Parses news source's RSS and saves new news into database
        /// </summary>
        /// <param name="newsSource">NewsSource object</param>
        /// <returns>Async Task object</returns>
        public async Task<List<NewsItemParseModel>> ParseNewsSource(NewsSource newsSource)
        {
            try
            {
                XElement xmlItems = await GetRssXml(newsSource.RssUrl);
                List<XElement> xmlElements = xmlItems.Descendants("item").ToList();
                return ParseNewsSourceRss(xmlElements);
            }
            catch (Exception e)
            {
                throw new FeedParsingException("Failed to parse RSS feed", e);
            }
        }

        /// <summary>
        /// Get the basic info of RSS source (channel name, etc.)
        /// </summary>
        /// <param name="rssUrl">RSS url</param>
        /// <returns>Task</returns>
        public async Task<NewsSource> ParseRssSource(string rssUrl)
        {
            try
            {
                XElement xmlItems = await GetRssXml(rssUrl);
                string sourceName = xmlItems.Descendants("channel")?.First()?.Element("title")?.Value;
                var newsSource = new NewsSource
                {
                    RssUrl = rssUrl,
                    Name = sourceName ?? "Unknown"
                };
                return newsSource;
            }
            catch (Exception e)
            {
                throw new FeedParsingException("Failed to parse RSS feed", e);
            }
        }

        /// <summary>
        /// Get the RSS channel xml
        /// </summary>
        /// <param name="rssUrl">Rss url</param>
        /// <returns>XElement</returns>
        private async Task<XElement> GetRssXml(string rssUrl)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string result = await httpClient.GetStringAsync(rssUrl);
                return XElement.Parse(result);
            }
            catch (Exception e)
            {
                throw new FeedParsingException($"Failed to parse RSS {rssUrl} source", e);
            }
        }

        /// <summary>
        /// Parses the incoming news source RSS feed XML
        /// </summary>
        /// <param name="xmlElements">The list of XML elements of RSS feed</param>
        /// <param name="newsSource">NewsSource object</param>
        /// <returns>List of NewsItem</returns>
        private List<NewsItemParseModel> ParseNewsSourceRss(List<XElement> xmlElements)
        {
            var newsItems = new List<NewsItemParseModel>();

            try
            {
                foreach (var rssItem in xmlElements)
                {
                    var rssItemDescription = rssItem.Element("description").Value;
                    
                    var newsItem = new NewsItemParseModel
                    {                       
                        Title = rssItem.Element("title").Value,
                        Description = CleanHtmlString(rssItemDescription),
                        DateAdded = DateTime.Parse(rssItem.Element("pubDate").Value),
                        LinkToSource = rssItem.Element("link").Value,
                        ImageUrl = ExtractFirstImage(rssItemDescription),
                        Categories = ExtractRssItemTags(rssItem)
                    };

                    newsItems.Add(newsItem);
                }

                return newsItems;
            }
            catch (Exception e)
            {
                throw new FeedParsingException("Failed to parse source", e);
            }
        }

        /// <summary>
        /// Performs the cleaning action for html string (removing some tags, etc.)
        /// </summary>
        /// <param name="html">Html string</param>
        /// <returns>Cleaned html string</returns>
        private string CleanHtmlString(string html)
        {
            string cleanHtmlString = Regex.Replace(html, "<.*?>", string.Empty, RegexOptions.IgnoreCase);
            if (cleanHtmlString.Length < 500)
            {
                return cleanHtmlString;
            }

            var croppedHtmlString = cleanHtmlString.Substring(0, 500);
            return $"{croppedHtmlString.Substring(0, croppedHtmlString.LastIndexOf(' '))}...";
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
