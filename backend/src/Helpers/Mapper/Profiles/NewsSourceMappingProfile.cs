using AutoMapper;
using NewsParser.API.Models;
using NewsParser.DAL.Models;

namespace NewsParser.Helpers.Mapper.Profiles
{
    /// <summary>
    /// AutoMapper profile for NewsSource entity
    /// </summary>
    public class NewsSourceMappingProfile: Profile
    {
        public NewsSourceMappingProfile()
        {
            CreateMap<NewsSource, NewsSourceApiModel>();

            CreateMap<NewsSource, NewsSourceApiListModel>()
                .ForMember(d => d.WebsiteUrl, opt => opt.MapFrom(s => s.RssUrl));
        }
    }
}
