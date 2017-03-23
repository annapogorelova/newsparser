using AutoMapper;
using NewsParser.API.Models;
using NewsParser.DAL.Models;
using NewsParser.Identity;

namespace NewsParser.Helpers.Mapper.Profiles
{
    public class UserMappingProfile: Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserApiModel>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => $"{s.FirstName} {s.LastName}"));

            CreateMap<ApplicationUser, User>();

            CreateMap<User, ApplicationUser>()
                .ForMember(a => a.NormalizedEmail, opt => opt.MapFrom(u => u.Email))
                .ForMember(a => a.NormalizedUserName, opt => opt.MapFrom(u => u.Email))
                .ForMember(a => a.PasswordHash, opt => opt.MapFrom(u => u.Password))
                .ForMember(a => a.UserName, opt => opt.MapFrom(u => u.Email));
        }
    }
}
