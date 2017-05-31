using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace NewsParser.FeedParser.Services.FeedSourceParser
{
    /// <summary>
    /// Interface that contains methods for parsing the feed XML
    /// </summary>
    public interface IFeedParser
    {
        XElement GetSourceElement(XElement xml);
        List<XElement> GetItems(XElement xml);
        
        string GetSourceTitle(XElement xml);
        string GetSourceDescription(XElement xml);
        string GetSourceImageUrl(XElement xml);
        string GetSourceWebsiteUrl(XElement xml);
        List<string> GetSourceCategories(XElement xml);
        string GetSourceUpdateInterval(XElement xml);
        string GetSourceLanguage(XElement xml);


        string GetItemId(XElement xml);
        string GetItemTitle(XElement xml);
        string GetItemLink(XElement xml);
        string GetItemDescription(XElement xml);
        string GetItemImageUrl(XElement xml);
        string GetItemAuthor(XElement xml);
        List<string> GetItemCategories(XElement xml);
        string GetItemDatePublished(XElement xml);
    }
}