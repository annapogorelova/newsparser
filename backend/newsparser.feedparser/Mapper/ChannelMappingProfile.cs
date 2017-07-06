using AutoMapper;
using NewsParser.DAL.Models;
using NewsParser.FeedParser.Models;
using NewsParser.FeedParser.Helpers;
using System;

namespace NewsParser.FeedParser.Mapper
{
    /// <summary>
    /// AutoMapper profile for Channel entity
    /// </summary>
    public class ChannelMappingProfile: Profile
    {
        public ChannelMappingProfile()
        {
            CreateMap<ChannelModel, Channel>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Name, 
                    opt => opt.MapFrom(s =>
                        string.IsNullOrEmpty(s.Name) ? "Untitled" :
                            s.Name
                            .RemoveTabulation(" ")
                            .RemoveHtmlTags()
                            .RemoveNonAlphanumericCharacters()
                            .CropString(100)))
                .ForMember(d => d.Description,
                    opt => opt.MapFrom(s =>
                        string.IsNullOrEmpty(s.Description) ? string.Empty :
                            s.Description
                            .RemoveTabulation(" ")
                            .RemoveHtmlTags()
                            .RemoveNonAlphanumericCharacters()
                            .CropString(255)))
                .ForMember(d => d.Users, opt => opt.Ignore())
                .ForMember(d => d.UpdateIntervalMinutes, opt => opt.Ignore())
                .AfterMap(SetUpdateInterval);
        }

        private void SetUpdateInterval(ChannelModel channelModel, Channel channel)
        {
            if(channelModel.UpdateIntervalMinutes != 0)
            {
                channel.UpdateIntervalMinutes = channelModel.UpdateIntervalMinutes;
            }
        }
    }
}