using Xunit;

namespace MoneyRemittance.IntegrationTests._SeedWork;

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<TestFixture>
{
}
