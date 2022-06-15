using TLM.Books._Infrastructure;
using TLM.Books.Domain.Entities;
using TLM.Books.IntegrationTests.Configurations;

namespace TLM.Books.IntegrationTests.Scenarios.Users;

public class UserApplicationFactory : BaseWebApplicationFactory
{
    public override string TestDataFolderPath => "Scenarios/Users/TestData";


    protected override void SeedData(BookDbContext context)
    {
        base.SeedData(context);
        
        Users = context.Set<User>().AddRangeFromFile($"{TestDataFolderPath}/Users.json");
    }
}