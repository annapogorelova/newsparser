using System.Threading.Tasks;
using newsparser.DAL.Models;

namespace NewsParser.Web.Auth.ExternalAuth
{
    public interface IExternalAuthService
    {
        Task<ExternalUser> VerifyAccessTokenAsync(string token, ExternalAuthProvider provider);
    }
}
