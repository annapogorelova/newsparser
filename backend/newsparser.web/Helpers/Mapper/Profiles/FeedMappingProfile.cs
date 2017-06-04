using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using NewsParser.API.Models;
using NewsParser.DAL.Models;

namespace NewsParser.Helpers.Mapper.Profiles
{
    /// <summary>
    /// AutoMapper profile for FeedItem entity
    /// </summary>
    public class FeedMappingProfile: Profile
    {
        public FeedMappingProfile()
        {
            CreateMap<FeedItem, FeedItemModel>()
                .ForMember(d => d.Channels, opt => opt.MapFrom(s => 
                    AutoMapper.Mapper.Map<List<ChannelModel>>(s.Channels.Select(ns => ns.Channel))))
                .ForMember(d => d.Tags, opt => opt.MapFrom(s => s.Tags.Select(t => t.Tag.Name)));
        }
    }
}
