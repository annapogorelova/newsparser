using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
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

        ApplicationUser FindUserByUserName(string userName);

        bool CheckUserPassword(ApplicationUser user, string password);

        Task<IdentityResult> CreateAsync(string email, string password);

        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);

        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);

        Task<IdentityResult> ConfirmEmail(ApplicationUser user, string confirmationToken);

        Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string passwordResetToken, string newPassword);

        Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);

        Task<IdentityResult> UpdateAsync(ApplicationUser user);
    }
}
