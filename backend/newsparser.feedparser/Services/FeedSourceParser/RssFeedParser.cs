using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using NewsParser.FeedParser.Exceptions;

namespace NewsParser.FeedParser.Services.FeedSourceParser
{
    /// <summary>
    /// RSS feed XML parser implementation
    /// </summary>
    public class RssFeedParser : IFeedParser
    {
        private readonly string[] ImageMIMETypes = new string[] 
        {
            "image/gif", "image/png", "image/jpeg", "image/bmp", "image/webp"
        };

        public string GetSourceDescription(XElement xml)
        {
            return xml.Element("description")?.Value;
        }

        public List<string> GetSourceCategories(XElement xml)
        {
            var categoryElements = xml.Elements("category").ToList();
            return categoryElements.Select(e => e.Value.ToLowerInvariant()).Distinct().ToList();
        }

        public string GetSourceLastUpdatedDate(XElement xml)
        {
            return xml.Element("lastBuildDate")?.Value;
        }

        public string GetSourceWebsiteUrl(XElement xml)
        {
            return xml.Element("link")?.Value;
        }

        public string GetSourceImageUrl(XElement xml)
        {
            if(xml.Descendants("image").Any())
            {
                return xml.Descendants("image").First().Element("url")?.Value;
            }

            return null;
        }

        public string GetSourceTitle(XElement xml)
        {
            return xml.Element("title")?.Value;
        }

        public string GetItemAuthor(XElement xml)
        {
            return xml.Element("author")?.Value;
        }

        public List<string> GetItemCategories(XElement xml)
        {
            var categoryElements = xml.Elements("category").ToList();
            return categoryElements.Select(e => e.Value.ToLowerInvariant()).Distinct().ToList();
        }

        public string GetItemDatePublished(XElement xml)
        {
            return xml.Element("pubDate")?.Value;
        }

        public string GetItemDescription(XElement xml)
        {
            var description = xml.Element("description")?.Value;
            if(string.IsNullOrEmpty(description))
            {
                return null;
            }

            return description;

        }

        public string GetItemId(XElement xml)
        {
            return xml.Element("guid")?.Value ?? GetItemLink(xml);
        }

        public string GetItemImageUrl(XElement xml)
        {
            string enclosureImageUrl = GetEnclosureImage(xml);
            if(!string.IsNullOrEmpty(enclosureImageUrl))
            {
                return enclosureImageUrl;
            }
            
            return !string.IsNullOrEmpty(xml.Element("description")?.Value) ? 
                ExtractFirstImage(xml.Element("description").Value) : null;
        }

        public string GetItemLink(XElement xml)
        {
            var guidElement = xml.Element("guid");
            if(guidElement != null && !string.IsNullOrEmpty(guidElement.Value) 
                && !string.IsNullOrEmpty(guidElement.Attribute("isPermaLink")?.Value))
            {
                bool isPermaLink = Convert.ToBoolean(guidElement.Attribute("isPermaLink").Value);
                if(isPermaLink)
                {
                    return guidElement.Value;
                }
            }
            
            return xml.Element("link")?.Value;
        }

        public string GetItemTitle(XElement xml)
        {
            return xml.Element("title")?.Value;
        }

        private string GetEnclosureImage(XElement xml)
        {
            var enclosure = xml.Element("enclosure");
            if (enclosure != null && !string.IsNullOrEmpty(enclosure.Attribute("type")?.Value)
                && !string.IsNullOrEmpty(enclosure.Attribute("url")?.Value)
                && ImageMIMETypes.Contains(enclosure.Attribute("type").Value))
            {
                return enclosure.Attribute("url").Value;
            }

            return null;
        }

        /// <summary>
        /// Extracts the first img tag's src attribute from html string
        /// </summary>
        /// <param name="html">Html string</param>
        /// <returns>First img tag's src attribute or null if no img tags found</returns>
        private string ExtractFirstImage(string html)
        {
            var match = Regex.Match(html, "<img.+?src=[\"'](.+?)[\"'].*?>", 
                RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value : null;
        }

        public List<XElement> GetItems(XElement xml)
        {
            return xml.Descendants("item").ToList();
        }

        public XElement GetSourceElement(XElement xml)
        {
            return xml.Descendants("channel").FirstOrDefault();
        }
    }
}