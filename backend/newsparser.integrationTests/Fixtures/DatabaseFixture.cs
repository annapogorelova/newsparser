using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NewsParser.DAL;

namespace NewsParser.IntegrationTests.Fixtures
{
    public class DatabaseFixture : IDisposable
    {
        private AppDbContext _dbContext { get; set; }
        
        public DatabaseFixture()
        {
        }

        public AppDbContext CreateDbContext()
        {
            var configuration = ServiceLocator.Instance.GetService(typeof(IConfiguration)) as IConfiguration;
            var connection = configuration.GetSection("ConnectionStrings")["TestAppDbContext"];
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseMySql(connection, b => b.MigrationsAssembly("newsparser.DAL"));
            _dbContext = new AppDbContext(optionsBuilder.Options);
            return _dbContext;
        }

        public void Dispose()
        {
            if(_dbContext != null)
            {
                _dbContext.Dispose();
            }
        }
    }
}