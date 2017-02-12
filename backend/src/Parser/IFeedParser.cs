﻿using System.Threading.Tasks;
using NewsParser.DAL.Models;

namespace NewsParser.Parser
{
    public interface IFeedParser
    {
        Task ParseNewsSource(NewsSource newsSource);
    }
}
