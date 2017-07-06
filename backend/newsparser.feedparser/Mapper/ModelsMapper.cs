using System;
using AutoMapper;

namespace NewsParser.FeedParser.Mapper
{
    /// <summary>
    /// Static class that performs the configuration of AutoMapper 
    /// </summary>
    public static class ModelsMapper
    {
        /// <summary>
        /// Automapper's main configuration method
        /// </summary>
        public static void Congifure()
        {
            AutoMapper.Mapper.Initialize(ConfigAction);
        }

        public static Action<IMapperConfigurationExpression> ConfigAction = cfg =>
        {
            cfg.AddProfile<FeedMappingProfile>();
            cfg.AddProfile<ChannelMappingProfile>();
        };
    }
}
