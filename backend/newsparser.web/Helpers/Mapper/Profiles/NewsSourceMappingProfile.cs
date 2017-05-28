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
                .ForMember(d => d.IsPrivate, opt => opt.Ignore())
                .ForMember(d => d.IsSubscribed, opt => opt.Ignore())
                .AfterMap(FinalizeSourceMapping);
        }

        private void FinalizeSourceMapping(NewsSource newsSource, NewsSourceSubscriptionModel model)
        {
            var newsSourceBusinessService = ServiceLocator.Instance.GetService<INewsSourceBusinessService>();
            model.IsSubscribed = newsSourceBusinessService.IsUserSubscribed(newsSource.Id, CurrentUser.GetCurrentUser().GetId());
            model.IsPrivate = newsSourceBusinessService.IsSourcePrivateToUser(newsSource.Id, CurrentUser.GetCurrentUser().GetId());
        }
    }
}
