﻿using System.Collections.Generic;
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
                .ForMember(d => d.Source, opt => opt.MapFrom(s => 
                    AutoMapper.Mapper.Map<NewsSourceApiListModel>(s.Source)))
                .ForMember(d => d.Tags, opt => opt.MapFrom(s => 
                    AutoMapper.Mapper.Map<List<NewsTagApiModel>>(s.NewsItemTags.Select(t => t.Tag))));
        }
    }
}