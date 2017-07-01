using System;
using System.Collections.Generic;
using NewsParser.DAL;

namespace NewsParser.API.V1.Models
{
    /// <summary>
    /// Class represents the FeedItem entity for GET API request
    /// </summary>
    public class FeedItemModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DatePublished { get; set; }
        public string LinkToSource { get; set; }
        public string ImageUrl { get; set; }
        public List<ChannelModel> Channels { get; set; }        
        public List<string> Tags { get; set; } 
    }
}
