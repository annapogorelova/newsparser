using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using NewsParser.FeedParser.Exceptions;
using NewsParser.FeedParser.Helpers;

namespace NewsParser.FeedParser.Services.FeedSourceParser
{
    public class AtomFeedParser : IFeedParser
    {
        public string GetItemAuthor(XElement xml)
        {
            var authorElement = GetElement(xml, "author");
            if(authorElement != null)
            {
                return GetElement(xml, "email")?.Value
                    ?? GetElement(xml, "name")?.Value;
            }

            return null;
        }

        public List<string> GetItemCategories(XElement xml)
        {
            return GetElements(xml, "category")
                .Where(e => !string.IsNullOrEmpty(e.Attribute("term")?.Value))
                .Select(e => e.Attribute("term").Value.ToLowerInvariant())
                .Distinct()
                .ToList();
        }

        public string GetItemDatePublished(XElement xml)
        {
            return GetElement(xml, "published")?.Value
                ?? GetElement(xml, "updated")?.Value;
        }

        public string GetItemDescription(XElement xml)
        {
            return GetElement(xml, "summary")?.Value;
        }

        public string GetItemId(XElement xml)
        {
            return GetElement(xml, "id")?.Value 
                ?? GetElement(xml, "link")?
                .Attribute("href")?.Value;
        }

        public string GetItemImageUrl(XElement xml)
        {
            var imageLink = GetElements(xml, "link")
                .Where(d => d.Attribute("rel")?.Value == "enclosure")
                .FirstOrDefault();
            return imageLink?.Value;
        }

        public string GetItemLink(XElement xml)
        {
            return GetElement(xml, "link")?.Attribute("href")?.Value;
        }

        public List<XElement> GetItems(XElement xml)
        {
            return GetElements(xml, "entry");
        }

        public string GetItemTitle(XElement xml)
        {
            return GetElement(xml, "title")?.Value;
        }

        public List<string> GetSourceCategories(XElement xml)
        {
            return GetElements(xml, "category")
                .Where(e => !string.IsNullOrEmpty(e.Attribute("term")?.Value))
                .Select(e => e.Attribute("term").Value.ToLowerInvariant())
                .Distinct()
                .ToList();
        }

        public string GetSourceDescription(XElement xml)
        {
            return GetElement(xml, "subtitle")?.Value;
        }

        public XElement GetSourceElement(XElement xml)
        {
            return xml;
        }

        public string GetSourceImageUrl(XElement xml)
        {
            return GetElement(xml, "logo")?.Value;
        }

        public string GetSourceLanguage(XElement xml)
        {
            var languageElement = GetElements(xml, "link")
                .FirstOrDefault(e => e.Attribute("hreflang") != null);
            if(languageElement == null)
            {
                return null;
            }

            var languageValue = languageElement.Attribute("hreflang").Value;
            return string.IsNullOrEmpty(languageValue) ? null : languageValue.ToLower().Split('-')[0];
        }

        public string GetSourceTitle(XElement xml)
        {
            return GetElement(xml, "title")?.Value;
        }

        public string GetSourceUpdateInterval(XElement xml)
        {
            return null;
        }

        public string GetSourceWebsiteUrl(XElement xml)
        {
            return GetElement(xml, "link")?.Attribute("href")?.Value;
        }

        private XElement GetElement(XElement xml, string name)
        {
            return xml.Elements().FirstOrDefault(e => e.Name.LocalName == name);   
        }

        private List<XElement> GetElements(XElement xml, string name)
        {
            return xml.Elements().Where(e => e.Name.LocalName == name).ToList();   
        } 
    }
}