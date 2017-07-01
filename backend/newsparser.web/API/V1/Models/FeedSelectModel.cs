using System;
using NewsParser.Helpers.ValidationAttributes;

namespace NewsParser.API.V1.Models
{
    /// <summary>
    /// Model contains a feed GET request parameters
    /// </summary>
    public class FeedSelectModel
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 5;

        public string Search { get; set; }

        [DigitsStringArray]
        public string[] Channels { get; set; } = null;

        public string[] Tags { get; set; } = null;
    }
}
