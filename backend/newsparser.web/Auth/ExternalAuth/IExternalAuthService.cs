using System.Threading.Tasks;

namespace NewsParser.Auth.ExternalAuth
{
    public interface IExternalAuthService
    {
        Task<ExternalUser> VerifyFacebookTokenAsync(string token);
    }
}
