using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using newsparser.DAL.Models;
using NewsParser.Auth.ExternalAuth;
using NewsParser.Identity.Models;

namespace NewsParser.Auth
{
    /// <summary>
    /// Interface contains authentication service methods declarations
    /// </summary>
    public interface IAuthService
    {
        Task<ClaimsPrincipal> GetUserPrincipalAsync(ApplicationUser user);

        Task<ClaimsPrincipal> GetSocialUserPrincipalAsync(ApplicationUser user, ExternalAuthProvider authProvider);

        AuthenticationTicket GetAuthTicket(ClaimsPrincipal principal);

        ApplicationUser GetCurrentUser();

        Task<ApplicationUser> CreateExternalUserAsync(ExternalUser externalUser, ExternalAuthProvider authProvider);

        Task<ApplicationUser> UpdateExternalUserAsync(ApplicationUser applicationUser, ExternalUser externalUser, ExternalAuthProvider authProvider);

        ApplicationUser FindUserByExternalId(string socialId, ExternalAuthProvider provider);

        ApplicationUser FindUserByEmail(string email);

        ApplicationUser FindExternalUser(ExternalUser user);

        bool CheckUserPassword(ApplicationUser user, string password);
    }
}
