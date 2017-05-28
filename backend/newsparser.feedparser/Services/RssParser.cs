using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Xml.Linq;
using NewsParser.DAL.Models;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NewsParser.FeedParser.Models;
using NewsParser.FeedParser.Exceptions;
using newsparser.FeedParser.Models;
using NewsParser.FeedParser.Helpers;

namespace NewsParser.FeedParser.Services
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
        public async Task<List<NewsItemModel>> ParseNewsSource(NewsSource newsSource)
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
        public async Task<NewsSourceModel> ParseRssSource(string rssUrl)
        {
            try
            {
                XElement xmlItems = await GetRssXml(rssUrl);
                XElement sourceInfo = xmlItems.Descendants("channel")?.First();
                if(sourceInfo == null)
                {
                    throw new FeedParsingException("Failed to parse RSS source");
                }

                var newsSourceModel = new NewsSourceModel
                {
                    RssUrl = rssUrl,
                    Name = sourceInfo.Element("title")?.Value ?? "Unknown",
                    Description = sourceInfo.Element("description")?.Value,
                    WebsiteUrl = sourceInfo.Element("link")?.Value
                };

                if(sourceInfo.Descendants("image").Any())
                {
                    newsSourceModel.ImageUrl = sourceInfo.Descendants("image").First()
                        .Element("url")?.Value;
                }

                if(sourceInfo.Element("lastBuildDate") != null && 
                    !string.IsNullOrEmpty(sourceInfo.Element("lastBuildDate").Value))
                {
                    DateTime lastBuildDate;
                    DateTime.TryParse(sourceInfo.Element("lastBuildDate").Value, out lastBuildDate);
                    newsSourceModel.LastBuildDate = lastBuildDate;
                }

                return newsSourceModel;
            }
            catch (Exception e)
            {
                throw e;
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
        private List<NewsItemModel> ParseNewsSourceRss(List<XElement> xmlElements)
        {
            var newsItems = new List<NewsItemModel>();

            try
            {
                foreach (var rssItem in xmlElements)
                {
                    var rssItemDescription = rssItem.Element("description")?.Value?.RemoveTabulation(" ");
                    string imageUrl = rssItemDescription != null ? 
                        ExtractFirstImage(rssItemDescription) : null;
                    
                    DateTime pubDate;
                    var result = DateTime.TryParse(rssItem.Element("pubDate").Value, out pubDate);
                    
                    if(!result)
                    {
                        pubDate = DateTime.UtcNow;
                    }
                    
                    var newsItem = new NewsItemModel
                    {                       
                        Title = rssItem.Element("title").Value,
                        Author = rssItem.Element("author")?.Value?.CropString(100),
                        Description = rssItemDescription?.RemoveHtmlTags().CropString(500),
                        DatePublished = pubDate.ToUniversalTime(),
                        LinkToSource = rssItem.Element("link").Value,
                        ImageUrl = imageUrl,
                        Categories = ExtractRssItemTags(rssItem)
                    };

                    var guidElement = rssItem.Element("guid");
                    if(guidElement != null)
                    {
                        var isPermaLinkAttribute = guidElement.Attribute("isPermaLink");
                        newsItem.Guid = new RssItemGuid
                        {
                            GuidString = guidElement.Value,
                            IsPermaLink = isPermaLinkAttribute != null ?
                                Convert.ToBoolean(isPermaLinkAttribute.Value) : true
                        };
                    }

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
