namespace newsparser.feedparser
{
    /// <summary>
    /// Interface contains a declaration of methods for updating the RSS feed
    /// </summary>
    public interface IFeedUpdater
    {
        /// <summary>
        /// Updates all feed
        /// </summary>
        void UpdateFeed();

        /// <summary>
        /// Updates specific source
        /// </summary>
        /// <param name="sourceId">Source id</param>
        void UpdateSource(int sourceId);
    }
}
