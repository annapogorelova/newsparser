using System.Linq;
using newsparser.DAL.Models;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.Repositories.Tokens
{
    /// <summary>
    /// Provides a functionality to access the Token entity data
    /// </summary>
    public interface ITokenRepository
    {
        /// <summary>
        /// Get token by id
        /// </summary>
        /// <param name="id">Token id</param>
        /// <returns>Token object</returns>
        Token GetTokenById(string id);

        /// <summary>
        /// Get tokens by subject
        /// </summary>
        /// <param name="subject">Subject string</param>
        /// <returns>IQueryable of Token</returns>
        IQueryable<Token> GetBySubject(string subject);

        /// <summary>
        /// Add token
        /// </summary>
        /// <param name="token">Token to add</param>
        /// <returns>Token object</returns>
        Token AddToken(Token token);

        /// <summary>
        /// Update a token
        /// </summary>
        /// <param name="user">Token object</param>
        void UpdateToken(Token token);

        /// <summary>
        /// Delete token
        /// </summary>
        /// <param name="user">Token object</param>
        void DeleteToken(Token token);
    }
}
