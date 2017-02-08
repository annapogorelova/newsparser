using AutoMapper;
using NewsParser.API.Models;
using NewsParser.DAL.Models;

namespace NewsParser.Helpers.Mapper.Profiles
{
    /// <summary>
    /// AutoMapper profile for NewsItem entity
    /// </summary>
    public class NewsMappingProfile: Profile
    {
        public NewsMappingProfile()
        {
            CreateMap<NewsItem, NewsItemApiModel>()
                .ForMember(d => d.SourceName, opt => opt.MapFrom(s => s.Source.Name));
        }
    }
}
