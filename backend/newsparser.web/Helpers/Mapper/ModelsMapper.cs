using System;
using AutoMapper;
using NewsParser.Helpers.Mapper.Profiles;

namespace NewsParser.Helpers.Mapper
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
            //AutoMapper.Mapper.AssertConfigurationIsValid();
        }

        public static Action<IMapperConfigurationExpression> ConfigAction = cfg =>
        {
            cfg.AddProfile<NewsMappingProfile>();
            cfg.AddProfile<NewsSourceMappingProfile>();
            cfg.AddProfile<UserMappingProfile>();
            cfg.AddProfile<TokenMappingProfile>();
        };
    }
}
