using Xunit;

namespace TLM.Books.IntegrationTests.Scenarios.Books;

[CollectionDefinition(nameof(BookCollectionFixtureDefinition))]
public class BookCollectionFixtureDefinition : ICollectionFixture<BookApplicationFactory>
{
    
}