using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NewsParser.API.Models;
using NewsParser.Auth;
using NewsParser.BL.Services.NewsSources;
using NewsParser.DAL.Models;
using NewsParser.Identity.Models;

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
            CreateMap<NewsSource, NewsSourceSubscriptionModel>()
                .ForMember(d => d.SubscribersCount, opt => opt.MapFrom(s => s.UsersSources.Count()))
                .ForMember(d => d.IsPrivate, opt => opt.Ignore())
                .ForMember(d => d.IsSubscribed, opt => opt.Ignore())
                .AfterMap(FinalizeSourceMapping);
        }

        private void FinalizeSourceMapping(NewsSource newsSource, NewsSourceSubscriptionModel model)
        {
            var newsSourceBusinessService = ServiceLocator.Instance.GetService<INewsSourceBusinessService>();
            var currentUserId = CurrentUser.GetCurrentUser().GetId();
            model.IsSubscribed = newsSource.UsersSources.Any(us => us.UserId == currentUserId);
            model.IsPrivate = newsSource.UsersSources.Any(us => us.UserId == currentUserId && us.IsPrivate);
        }
    }
}
