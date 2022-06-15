using TLM.Books._Infrastructure;
using TLM.Books.Domain.Entities;
using TLM.Books.IntegrationTests.Configurations;

namespace TLM.Books.IntegrationTests.Scenarios.Books;

public class BookApplicationFactory : BaseWebApplicationFactory
{
    public override string TestDataFolderPath => "Scenarios/Books/TestData";


    protected override void SeedData(BookDbContext context)
    {
        base.SeedData(context);
        
        Books = context.Set<Book>().AddRangeFromFile($"{TestDataFolderPath}/Books.json");
    }
}