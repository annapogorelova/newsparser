using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NewsParser.API.Models;
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
            CreateMap<NewsSource, NewsSourceApiModel>()
                .ForMember(m => m.IsSubscribed, opt => opt.Ignore())
                .AfterMap(SetSubscribedState);
        }

        private void SetSubscribedState(NewsSource newsSource, NewsSourceApiModel model)
        {
            var userManager = ServiceLocator.Instance.GetService<UserManager<ApplicationUser>>();
            var httpContextAccessor = ServiceLocator.Instance.GetService<IHttpContextAccessor>();
            var user = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result;
            model.IsSubscribed = newsSource.Users == null ? false : newsSource.Users.Any(u => u.UserId == user.GetId());
        }
    }
}
