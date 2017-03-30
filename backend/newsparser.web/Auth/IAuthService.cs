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

        Task<ApplicationUser> SaveExternalUserAsync(ExternalUser externalUser, ExternalAuthProvider authProvider);

        ApplicationUser FindUserBySocialId(string socialId, ExternalAuthProvider provider);

        ApplicationUser FindUserByName(string username);

        bool CheckUserPassword(ApplicationUser user, string password);
    }
}
