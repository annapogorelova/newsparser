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
            AutoMapper.Mapper.Initialize(cfg => cfg.AddProfile(new NewsMappingProfile()));
            AutoMapper.Mapper.AssertConfigurationIsValid();
        }
    }
}
