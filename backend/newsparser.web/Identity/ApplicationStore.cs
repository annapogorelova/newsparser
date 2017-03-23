using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OpenIddict.Core;
using OpenIddict.Models;

namespace NewsParser.Identity
{
    public class ApplicationStore: IOpenIddictApplicationStore<OpenIddictApplication>
    {
        public Task<OpenIddictApplication> CreateAsync(OpenIddictApplication application, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(OpenIddictApplication application, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<OpenIddictApplication> FindByIdAsync(string identifier, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<OpenIddictApplication> FindByClientIdAsync(string identifier, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<OpenIddictApplication> FindByLogoutRedirectUri(string url, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetClientIdAsync(OpenIddictApplication application, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetClientTypeAsync(OpenIddictApplication application, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetDisplayNameAsync(OpenIddictApplication application, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetHashedSecretAsync(OpenIddictApplication application, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetIdAsync(OpenIddictApplication application, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetLogoutRedirectUriAsync(OpenIddictApplication application, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRedirectUriAsync(OpenIddictApplication application, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetTokensAsync(OpenIddictApplication application, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetClientTypeAsync(OpenIddictApplication application, string type, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetHashedSecretAsync(OpenIddictApplication application, string hash, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(OpenIddictApplication application, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
