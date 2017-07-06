using System.Linq;
using FluentScheduler;
using NewsParser.FeedParser;
using Microsoft.Extensions.DependencyInjection;
using NewsParser.Scheduler;
using NewsParser.BL.Services.Channels;
using NewsParser.FeedParser.Exceptions;
using NewsParser.FeedParser.Services;
using System;
using Microsoft.Extensions.Logging;

namespace NewsParser.Scheduler
{
    /// <summary>
    /// Class represents a job that's to be scheduled to update RSS feed
    /// </summary>
    public class FeedUpdateJob : IJob
    {
        private readonly IFeedUpdater _feedUpdater;
        private readonly IChannelDataService _channelDataService;
        private readonly ILogger<FeedUpdateJob> _log;
        private readonly object _feedUpadteLock = new object();

        public FeedUpdateJob()
        {
            _feedUpdater = ServiceLocator.Instance.GetService<IFeedUpdater>();
            _channelDataService = ServiceLocator.Instance.GetService<IChannelDataService>();
            _log = ServiceLocator.Instance.GetService<ILogger<FeedUpdateJob>>();
        }

        public void Execute()
        {
            lock (_feedUpadteLock)
            {
                try
                {
                    var channels = _channelDataService.GetForUpdate();
                    if(!channels.Any())
                    {
                        _log.LogInformation("No channels to update.");
                        return;
                    }

                    foreach (var channel in channels)
                    {
                        _feedUpdater.UpdateChannel(channel.Id);
                    }
                }
                catch (FeedParsingException e)
                {
                    throw new JobExecutionException($"Failed to update the feed", e);
                }
            }
        }
    }
}
