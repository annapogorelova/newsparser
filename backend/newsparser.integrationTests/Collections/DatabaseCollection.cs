using NewsParser.IntegrationTests.Fixtures;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace NewsParser.IntegrationTests.Collections
{
    [CollectionDefinition("Database collection")]
    public class DatabaseCollection: ICollectionFixture<DatabaseFixture>
    {
    }
}