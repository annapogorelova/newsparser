using System.Collections.Generic;
using AutoMapper;
using newsparser.DAL.Models;
using NewsParser.API.Models;
using NewsParser.Auth.ExternalAuth;
using NewsParser.DAL.Models;
using NewsParser.Identity.Models;

namespace NewsParser.Helpers.Mapper.Profiles
{
    public class UserMappingProfile: Profile
    {
        public UserMappingProfile()
        {
            CreateMap<ApplicationUser, AccountModel>()
                .ForMember(m => m.Id, opt => opt.MapFrom(u => u.GetId()))
                .ForMember(m => m.HasPassword, opt => opt.MapFrom(u => !string.IsNullOrEmpty(u.PasswordHash)));

            CreateMap<UserExternalId, ExternalIdModel>();

            CreateMap<ExternalIdModel, UserExternalId>();

            CreateMap<ApplicationUser, User>()
                .ForMember(u => u.Id, opt => opt.Ignore())
                .ForMember(u => u.Password, opt => opt.MapFrom(a => a.PasswordHash ?? string.Empty))
                .ForMember(u => u.UserExternalIds, opt => opt.MapFrom(a => AutoMapper.Mapper.Map<List<ExternalIdModel>, List<UserExternalId>>(a.ExternalIds)));

            CreateMap<User, ApplicationUser>()
                .ForMember(a => a.Id, opt => opt.MapFrom(u => u.Id.ToString()))
                .ForMember(a => a.NormalizedEmail, opt => opt.MapFrom(u => u.Email.ToLower()))
                .ForMember(a => a.NormalizedUserName, opt => opt.MapFrom(u => u.UserName.ToLower()))
                .ForMember(a => a.PasswordHash, opt => opt.MapFrom(u => u.Password))
                .ForMember(a => a.ExternalIds, opt => opt.MapFrom(u => 
                    u.UserExternalIds != null ? 
                        AutoMapper.Mapper.Map<List <UserExternalId>, List <ExternalIdModel>>(u.UserExternalIds) : 
                        new List<ExternalIdModel>()));

            CreateMap<ExternalUser, ApplicationUser>()
                .ForMember(a => a.Id, opt => opt.Ignore())
                .ForMember(a => a.PasswordHash, opt => opt.Ignore());
        }
    }
}
