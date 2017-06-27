using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using NewsParser.DAL.Models;
using NewsParser.DAL.Repositories.Tokens;
using OpenIddict.Core;
using OpenIddict.Models;

namespace NewsParser.Web.Identity.Stores
{
    public class TokenStore: IOpenIddictTokenStore<OpenIddictToken>
    {
        private readonly ITokenRepository _tokenRepository;
        
        public TokenStore(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public Task<OpenIddictToken> CreateAsync(OpenIddictToken token, CancellationToken cancellationToken)
        {
            var addedToken = _tokenRepository.AddToken(Mapper.Map<OpenIddictToken, Token>(token));
            return Task.FromResult(Mapper.Map<Token, OpenIddictToken>(addedToken));
        }

        public Task<OpenIddictToken> CreateAsync(string type, string subject, CancellationToken cancellationToken)
        {
            var addedToken = _tokenRepository.AddToken(new Token(){
                Type = type,
                Subject = subject
            });
            return Task.FromResult(Mapper.Map<Token, OpenIddictToken>(addedToken));
        }

        public Task<OpenIddictToken> FindByIdAsync(string identifier, CancellationToken cancellationToken)
        {
            var token = _tokenRepository.GetTokenById(identifier);
            return Task.FromResult(Mapper.Map<Token, OpenIddictToken>(token));
        }

        public Task<OpenIddictToken[]> FindBySubjectAsync(string subject, CancellationToken cancellationToken)
        {
            var tokens = _tokenRepository.GetBySubject(subject).ToList();
            var tokenModels = Mapper.Map<List<Token>, List<OpenIddictToken>>(tokens);
            return Task.FromResult(tokenModels.ToArray());
        }

        public Task<string> GetIdAsync(OpenIddictToken token, CancellationToken cancellationToken)
        {
            return Task.FromResult(token.Id);
        }

        public Task<string> GetTokenTypeAsync(OpenIddictToken token, CancellationToken cancellationToken)
        {
            return Task.FromResult(token.Type);
        }

        public Task<string> GetSubjectAsync(OpenIddictToken token, CancellationToken cancellationToken)
        {
            return Task.FromResult(token.Subject);
        }

        public Task RevokeAsync(OpenIddictToken token, CancellationToken cancellationToken)
        {
            var tokenToDelete = _tokenRepository.GetTokenById(token.Id);
            _tokenRepository.DeleteToken(tokenToDelete);
            return Task.CompletedTask;
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
            var tokenToUpdate = Mapper.Map<OpenIddictToken, Token>(token);
            _tokenRepository.UpdateToken(tokenToUpdate);
            return Task.CompletedTask;
        }
    }
}
