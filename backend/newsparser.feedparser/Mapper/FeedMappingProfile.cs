using AutoMapper;
using NewsParser.DAL.Models;
using NewsParser.FeedParser.Models;
using NewsParser.FeedParser.Helpers;

namespace NewsParser.FeedParser.Mapper
{
    /// <summary>
    /// AutoMapper profile for FeedItem entity
    /// </summary>
    public class FeedMappingProfile: Profile
    {
        public FeedMappingProfile()
        {
            CreateMap<FeedItem, FeedItem>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Tags, opt => opt.Ignore())
                .ForMember(d => d.Channels, opt => opt.Ignore())
                .ForMember(d => d.DateAdded, opt => opt.Ignore());

            CreateMap<FeedItemModel, FeedItem>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Tags, opt => opt.Ignore())
                .ForMember(d => d.Channels, opt => opt.Ignore())
                .ForMember(d => d.ImageUrl, opt => 
                    opt.Condition(s => 
                        !string.IsNullOrEmpty(s.ImageUrl) && 
                        s.ImageUrl.Length <= Constants.MaxUrlLength))
                .ForMember(d => d.Author, opt => opt.Condition(s => !string.IsNullOrEmpty(s.ImageUrl)))
                .ForMember(d => d.LinkToSource, opt => opt.Condition(s => !string.IsNullOrEmpty(s.LinkToSource)))
                .ForMember(d => d.Description,
                    opt => opt.MapFrom(s => 
                        string.IsNullOrEmpty(s.Description) ? string.Empty : 
                                s.Description
                                .RemoveTabulation(" ")
                                .RemoveHtmlTags()
                                .RemoveNonAlphanumericCharacters()
                                .CropString(Constants.MaxFeedItemDescriptionsLength)))
                .ForMember(d => d.Title,
                    opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Title) ? "Untitled" : 
                                s.Title
                                .RemoveTabulation(" ")
                                .RemoveHtmlTags()
                                .RemoveNonAlphanumericCharacters()
                                .CropString(Constants.MaxFeedItemTitleLength)))
                .ForMember(d => d.Guid, opt => opt.MapFrom(s => s.Id.CropString(Constants.MaxFeedItemGuidLength)));
        }
    }
}