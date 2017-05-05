using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using newsparser.DAL.Models;
using NewsParser.DAL.Models;

namespace NewsParser.DAL.Repositories.Tokens
{
    public class TokenRepository : ITokenRepository
    {
        private readonly AppDbContext _dbContext;

        public TokenRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Token AddToken(Token token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token), "Token cannot be null");
            }

            _dbContext.Tokens.Add(token);
            _dbContext.SaveChanges();
            return _dbContext.Entry(token).Entity;
        }

        public void DeleteToken(Token token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token), "Token cannot be null");
            }

            _dbContext.Remove(token);
            _dbContext.SaveChanges();
        }

        public IQueryable<Token> GetBySubject(string subject)
        {
            return _dbContext.Tokens.Where(t => t.Subject == subject);
        }

        public Token GetTokenById(string id)
        {
            return _dbContext.Tokens.Find(id);
        }

        public void UpdateToken(Token token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token), "User cannot be null");
            }

            _dbContext.Entry(token).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}