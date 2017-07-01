using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NewsParser.API.V1.Models;
using NewsParser.BL.Services.Channels;
using NewsParser.DAL.Models;
using NewsParser.Web.Identity.Models;
using NewsParser.Web.Auth;

namespace NewsParser.Helpers.Mapper.Profiles
{
    /// <summary>
    /// AutoMapper profile for Channel entity
    /// </summary>
    public class ChannelMappingProfile: Profile
    {
        public ChannelMappingProfile()
        {
            CreateMap<Channel, ChannelModel>();
            CreateMap<Channel, ChannelSubscriptionModel>()
                .ForMember(d => d.SubscribersCount, opt => opt.MapFrom(s => s.Users.Count()))
                .ForMember(d => d.IsPrivate, opt => opt.Ignore())
                .ForMember(d => d.IsSubscribed, opt => opt.Ignore())
                .AfterMap(FinalizeSourceMapping);
        }

        private void FinalizeSourceMapping(Channel newsSource, ChannelSubscriptionModel model)
        {
            var newsSourceBusinessService = ServiceLocator.Instance.GetService<IChannelDataService>();
            var currentUserId = CurrentUser.GetCurrentUser().GetId();
            model.IsSubscribed = newsSource.Users.Any(us => us.UserId == currentUserId);
            model.IsPrivate = newsSource.Users.Any(us => us.UserId == currentUserId && us.IsPrivate);
        }
    }
}
