using Xunit;

namespace TLM.Books.IntegrationTests.Scenarios.Users;

[CollectionDefinition(nameof(UserCollectionFixtureDefinition))]
public class UserCollectionFixtureDefinition : ICollectionFixture<UserApplicationFactory>
{
}