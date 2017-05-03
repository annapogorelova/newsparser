using System.Collections.Generic;
using System.Linq;
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
                .ForMember(d => d.Sources, opt => opt.MapFrom(s => 
                    AutoMapper.Mapper.Map<List<NewsSourceApiModel>>(s.Sources.Select(ns => ns.Source))))
                .ForMember(d => d.Tags, opt => opt.MapFrom(s => s.Tags.Select(t => t.Tag.Name)));
        }
    }
}
