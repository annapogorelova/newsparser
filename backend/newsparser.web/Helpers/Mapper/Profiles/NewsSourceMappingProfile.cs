using System.Linq;
using AutoMapper;
using NewsParser.API.Models;
using NewsParser.DAL.Models;

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
            // TODO: GetCurrentUser
            int userId = 2;
            model.IsSubscribed = newsSource.Users.Any(u => u.UserId == userId);
        }
    }
}
