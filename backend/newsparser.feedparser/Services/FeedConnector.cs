using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using NewsParser.DAL.Models;
using NewsParser.FeedParser.Exceptions;
using NewsParser.FeedParser.Helpers;
using NewsParser.FeedParser.Models;
using NewsParser.FeedParser.Services.FeedSourceParser;

namespace NewsParser.FeedParser.Services
{
    public class FeedConnector : IFeedConnector
    {
        private readonly Dictionary<FeedFormat, IFeedParser> _feedParsers = 
            new Dictionary<FeedFormat, IFeedParser>
        {
            { FeedFormat.RSS, new RssFeedParser() },
            { FeedFormat.Atom, new AtomFeedParser() }
        };
        
        public async Task<List<FeedItem>> GetFeed(string feedUrl, FeedFormat feedFormat)
        {
            try
            {
                XElement feedXml = await GetFeedXml(feedUrl);
                var feedItemsXml = _feedParsers[feedFormat].GetItems(feedXml);
                var feedItemsList = new List<FeedItem>();

                foreach(var feedItemXml in feedItemsXml)
                {
                    var feedItem = new FeedItem
                    {
                        Id = _feedParsers[feedFormat].GetItemId(feedItemXml),
                        Title = _feedParsers[feedFormat].GetItemTitle(feedItemXml),
                        Author = _feedParsers[feedFormat].GetItemAuthor(feedItemXml)?.CropString(100),
                        Categories = _feedParsers[feedFormat].GetItemCategories(feedItemXml),
                        ImageUrl = _feedParsers[feedFormat].GetItemImageUrl(feedItemXml),
                        Link = _feedParsers[feedFormat].GetItemLink(feedItemXml),
                        Description = _feedParsers[feedFormat].GetItemDescription(feedItemXml)
                    };

                    string datePublishedString = _feedParsers[feedFormat].GetItemDatePublished(feedItemXml);
                    if(!string.IsNullOrEmpty(datePublishedString))
                    {
                        DateTime datePublished;
                        var succeeded = DateTime.TryParse(datePublishedString, out datePublished);
                        feedItem.DatePublished = succeeded ? datePublished : DateTime.UtcNow;
                    }

                    feedItemsList.Add(feedItem);
                }

                return feedItemsList;
            }
            catch (Exception e)
            {
                throw new FeedParsingException("Failed to parse RSS feed", e);
            }
        }

        public async Task<FeedSource> GetFeedSource(string feedUrl)
        {
            try
            {
                XElement feedXml = await GetFeedXml(feedUrl);
                var feedFormat = DetectFeedFormat(feedXml);
                XElement sourceElement = _feedParsers[feedFormat].GetSourceElement(feedXml);

                if(sourceElement == null)
                {
                    throw new FeedParsingException("Failed to parse RSS source");
                }

                var feedSource = new FeedSource
                {
                    FeedUrl = feedUrl,
                    Name = _feedParsers[feedFormat].GetSourceTitle(sourceElement),
                    Description = _feedParsers[feedFormat].GetSourceDescription(sourceElement),
                    WebsiteUrl = _feedParsers[feedFormat].GetSourceWebsiteUrl(sourceElement),
                    FeedFormat = feedFormat,
                    ImageUrl = _feedParsers[feedFormat].GetSourceImageUrl(sourceElement)
                };

                var sourceUpdateInterval = _feedParsers[feedFormat].GetSourceUpdateInterval(sourceElement);
                if(sourceUpdateInterval != null)
                {
                    feedSource.UpdateIntervalMinutes = Convert.ToInt32(sourceUpdateInterval);
                }

                return feedSource;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task<XElement> GetFeedXml(string rssUrl)
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

        private FeedFormat DetectFeedFormat(XElement xml)
        {
            if(xml.Name.LocalName == "rss" || xml.Element("channel") != null)
            {
                return FeedFormat.RSS;
            }
            else if(xml.Name.LocalName == "feed"
                && xml.Name.NamespaceName == "http://www.w3.org/2005/Atom")
            {
                return FeedFormat.Atom;
            }

            throw new UnsupportedFeedFormatException("Feed format is not supported");
        }
    }
}