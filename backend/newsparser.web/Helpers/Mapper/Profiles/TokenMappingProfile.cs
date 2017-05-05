using System.Collections.Generic;
using AutoMapper;
using newsparser.DAL.Models;
using NewsParser.API.Models;
using NewsParser.Auth.ExternalAuth;
using NewsParser.DAL.Models;
using NewsParser.Identity.Models;
using OpenIddict.Models;

namespace NewsParser.Helpers.Mapper.Profiles
{
    public class TokenMappingProfile: Profile
    {
        public TokenMappingProfile()
        {
            CreateMap<Token, OpenIddictToken>();

            CreateMap<OpenIddictToken, Token>();
        }
    }
}
