using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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
        private readonly IFeedProvider _feedProvider;

        public FeedConnector(IFeedProvider feedProvider)
        {
            _feedProvider = feedProvider;
        }

        private readonly Dictionary<FeedFormat, IFeedParser> _feedParsers = 
            new Dictionary<FeedFormat, IFeedParser>
        {
            { FeedFormat.RSS, new RssFeedParser() },
            { FeedFormat.Atom, new AtomFeedParser() }
        };
        
        public async Task<List<FeedItemModel>> ParseFeed(string feedUrl, FeedFormat feedFormat)
        {
            try
            {
                var feedXml = await _feedProvider.GetFeedXml(feedUrl);
                var feedParser = _feedParsers[feedFormat];
                var feedItemsXml = feedParser.GetItems(feedXml);
                var feedItemsList = new List<FeedItemModel>();

                foreach(var feedItemXml in feedItemsXml)
                {
                    var feedItem = new FeedItemModel
                    {
                        Id = feedParser.GetItemId(feedItemXml),
                        Title = feedParser.GetItemTitle(feedItemXml),
                        Author = feedParser.GetItemAuthor(feedItemXml)?.CropString(100),
                        Categories = feedParser.GetItemCategories(feedItemXml),
                        ImageUrl = feedParser.GetItemImageUrl(feedItemXml),
                        LinkToSource = feedParser.GetItemLink(feedItemXml),
                        Description = feedParser.GetItemDescription(feedItemXml)
                    };

                    string datePublishedString = feedParser.GetItemDatePublished(feedItemXml);
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

        public async Task<ChannelModel> ParseFeedSource(string feedUrl)
        {
            try
            {
                XElement feedXml = await _feedProvider.GetFeedXml(feedUrl);
                var feedFormat = DetectFeedFormat(feedXml);
                XElement sourceElement = _feedParsers[feedFormat].GetSourceElement(feedXml);

                if(sourceElement == null)
                {
                    throw new FeedParsingException("Failed to parse RSS source");
                }

                var feedParser = _feedParsers[feedFormat];

                var feedSource = new Models.ChannelModel
                {
                    FeedUrl = feedUrl,
                    Name = feedParser.GetSourceTitle(sourceElement),
                    Description = feedParser.GetSourceDescription(sourceElement),
                    WebsiteUrl = feedParser.GetSourceWebsiteUrl(sourceElement),
                    FeedFormat = feedFormat,
                    ImageUrl = feedParser.GetSourceImageUrl(sourceElement)
                };

                var language = feedParser.GetSourceLanguage(sourceElement);
                if(!string.IsNullOrEmpty(language) && 
                    Regex.Matches(language, "^[a-z]{2}$", RegexOptions.IgnoreCase).Count > 0)
                {
                    feedSource.Language = language;
                }

                var sourceUpdateInterval = feedParser.GetSourceUpdateInterval(sourceElement);
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