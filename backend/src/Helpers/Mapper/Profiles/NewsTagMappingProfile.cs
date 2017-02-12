using AutoMapper;
using NewsParser.API.Models;
using NewsParser.DAL.Models;

namespace NewsParser.Helpers.Mapper.Profiles
{
    /// <summary>
    /// AutoMapper profile for NewsTag entity
    /// </summary>
    public class NewsTagMappingProfile: Profile
    {
        public NewsTagMappingProfile()
        {
            CreateMap<NewsTag, NewsTagApiModel>();
        }
    }
}
