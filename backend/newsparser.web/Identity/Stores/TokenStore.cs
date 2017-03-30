using System.Threading;
using System.Threading.Tasks;
using OpenIddict.Core;
using OpenIddict.Models;

namespace NewsParser.Identity.Stores
{
    public class TokenStore: IOpenIddictTokenStore<OpenIddictToken>
    {
        public Task<OpenIddictToken> CreateAsync(OpenIddictToken token, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<OpenIddictToken> CreateAsync(string type, string subject, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<OpenIddictToken> FindByIdAsync(string identifier, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<OpenIddictToken[]> FindBySubjectAsync(string subject, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetIdAsync(OpenIddictToken token, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetTokenTypeAsync(OpenIddictToken token, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetSubjectAsync(OpenIddictToken token, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task RevokeAsync(OpenIddictToken token, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetAuthorizationAsync(OpenIddictToken token, string identifier, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetClientAsync(OpenIddictToken token, string identifier, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(OpenIddictToken token, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
