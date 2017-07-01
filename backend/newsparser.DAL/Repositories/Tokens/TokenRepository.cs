using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using newsparser.DAL.Models;
using NewsParser.DAL.Exceptions;
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

            try
            {
                _dbContext.Remove(token);
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                HandleConcurrencyException(ex);
                _dbContext.SaveChanges();
            }
        }

        private void HandleConcurrencyException(DbUpdateConcurrencyException ex)
        {
            foreach (var entry in ex.Entries)
            {
                if (entry.Entity is Token)
                {
                    // Using a NoTracking query means we get the entity but it is not tracked by the context
                    // and will not be merged with existing entities in the context.
                    var databaseEntity = _dbContext.Tokens.AsNoTracking()
                        .FirstOrDefault(p => p.Id == ((Token)entry.Entity).Id);
                    if(databaseEntity == null)
                    {
                        throw new DataLayerException("Entry does not exist in database");
                    }
                    var databaseEntry = _dbContext.Entry(databaseEntity);

                    foreach (var property in entry.Metadata.GetProperties())
                    {
                        var originalValue = entry.Property(property.Name).OriginalValue;
                        entry.Property(property.Name).CurrentValue = originalValue;
                        entry.Property(property.Name).OriginalValue = databaseEntry.Property(property.Name).CurrentValue;
                    }
                }
                else
                {
                    throw new DataLayerException("Failed to update the database entry: " + entry.Metadata.Name);
                }
            }
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

            try
            {                
                _dbContext.Entry(token).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                HandleConcurrencyException(ex);
                _dbContext.SaveChanges();
            }
        }
    }
}