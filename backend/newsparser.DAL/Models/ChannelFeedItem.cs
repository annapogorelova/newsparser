using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsParser.DAL.Models
{
    public class ChannelFeedItem
    {
        public int Id { get; set; }

        public int ChannelId { get;set; }

        public Channel Channel { get; set; }

        public int FeedItemId { get; set; }

        public FeedItem FeedItem { get; set; }
    }
}
