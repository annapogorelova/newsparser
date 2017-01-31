using AutoMapper;
using NewsParser.API.Models;
using NewsParser.DAL.Models;

namespace NewsParser.Helpers.Mapper.Profiles
{
    public class UserMappingProfile: Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserApiModel>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => $"{s.FirstName} {s.LastName}"));
        }
    }
}
