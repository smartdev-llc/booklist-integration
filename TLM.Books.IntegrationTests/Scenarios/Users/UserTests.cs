using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using TLM.Books.Application.Features.BookFeature.Commands;
using TLM.Books.Application.Features.UserFeature.Commands;
using TLM.Books.Application.Models;
using TLM.Books.Common.Error;
using TLM.Books.IntegrationTests.Configurations;
using Xunit;

namespace TLM.Books.IntegrationTests.Scenarios.Users;

[Collection(nameof(UserCollectionFixtureDefinition))]
public class UserTests
{
    private readonly UserApplicationFactory _factory;
    private string Endpoint => "api/users";

    public UserTests(UserApplicationFactory factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task QueryUser_GetAllUsers_ShouldBeSuccess()
    {
        var client = _factory.CreateDefaultClient();
        
        var response = await client.GetAsync($"{Endpoint}");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.GetContentAsync<MethodResult<IEnumerable<UserView>>>();
        Assert.NotNull(result);
        Assert.True(result.IsOK);
        Assert.NotNull(result.Result);
        Assert.NotEmpty(result.Result);
    }
    
    [Fact]
    public async Task AddUser_ValidData_ShouldBeSuccess()
    {
        var client = _factory.CreateDefaultClient();

        var createUserCommand = new CreateUserCommand
        {
            Name = "John Wick"
        };
        var request = new HttpRequestMessage(HttpMethod.Post, Endpoint);
        request.SetContent(createUserCommand);
        var response = await client.SendAsync(request);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.GetContentAsync<MethodResult<UserView>>();
        Assert.NotNull(result);
        Assert.True(result.IsOK);
        Assert.NotNull(result.Result);
        Assert.Equal(createUserCommand.Name, result.Result.Name);
    }
    
    [Fact]
    public async Task UpdateUser_ValidData_ShouldBeSuccess()
    {
        var client = _factory.CreateDefaultClient();

        var createUserCommand = new CreateUserCommand
        {
            Name = "John Cena"
        };
        var request = new HttpRequestMessage(HttpMethod.Post, Endpoint);
        request.SetContent(createUserCommand);
        var response = await client.SendAsync(request);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.GetContentAsync<MethodResult<UserView>>();
        Assert.NotNull(result);
        Assert.True(result.IsOK);
        Assert.NotNull(result.Result);
        Assert.Equal(createUserCommand.Name, result.Result.Name);

        var updateUserCommand = new UpdateUserCommand
        {
            Name = "Updated"
        };
        var updateRequest = new HttpRequestMessage(HttpMethod.Put, $"{Endpoint}/{result.Result.Id}");
        updateRequest.SetContent(updateUserCommand);
        var updateResponse = await client.SendAsync(updateRequest);
        
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
        var updateResult = await updateResponse.GetContentAsync<MethodResult<UserView>>();
        Assert.NotNull(updateResult);
        Assert.True(updateResult.IsOK);
        Assert.NotNull(updateResult.Result);
        Assert.Equal(updateUserCommand.Name, updateResult.Result.Name);
    }
    
    [Fact]
    public async Task DeleteUser_ValidData_ShouldBeSuccess()
    {
        var client = _factory.CreateDefaultClient();

        var createUserCommand = new CreateUserCommand
        {
            Name = "John Delete"
        };
        var request = new HttpRequestMessage(HttpMethod.Post, Endpoint);
        request.SetContent(createUserCommand);
        var response = await client.SendAsync(request);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.GetContentAsync<MethodResult<UserView>>();
        Assert.NotNull(result);
        Assert.True(result.IsOK);
        Assert.NotNull(result.Result);
        Assert.Equal(createUserCommand.Name, result.Result.Name);

        var deleteeUserCommand = new DeleteUserCommand();
        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"{Endpoint}/{result.Result.Id}");
        deleteRequest.SetContent(deleteeUserCommand);
        var deleteResponse = await client.SendAsync(deleteRequest);
        
        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        var deleteResult = await deleteResponse.GetContentAsync<MethodResult<UserView>>();
        Assert.NotNull(deleteResult);
        Assert.True(deleteResult.IsOK);
        Assert.NotNull(deleteResult.Result);
        Assert.Equal(result.Result.Id, deleteResult.Result.Id);
    }
    
    [Fact]
    public async Task User_AddReadBookList_ShouldBeSuccess()
    {
        var client = _factory.CreateDefaultClient();

        var createUserCommand = new CreateUserCommand
        {
            Name = "John Book"
        };
        var request = new HttpRequestMessage(HttpMethod.Post, Endpoint);
        request.SetContent(createUserCommand);
        var response = await client.SendAsync(request);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.GetContentAsync<MethodResult<UserView>>();
        Assert.NotNull(result);
        Assert.True(result.IsOK);
        Assert.NotNull(result.Result);
        Assert.Equal(createUserCommand.Name, result.Result.Name);
        
        // create new book
        var createBookCommand = new CreateBookCommand
        {
            Author = "Harry Potter",
            Name = "After all this time",
            ISBN = "Test"
        };
        var createBookRequest = new HttpRequestMessage(HttpMethod.Post, "api/Books");
        createBookRequest.SetContent(createBookCommand);
        var createBookResponse = await client.SendAsync(createBookRequest);
        Assert.Equal(HttpStatusCode.OK, createBookResponse.StatusCode);
        var createBookResult = await createBookResponse.GetContentAsync<MethodResult<BookView>>();
        Assert.NotNull(createBookResult);
        Assert.NotNull(createBookResult.Result);
        
        var addBookToUserComand = new AddBookToUserComand()
        {
            UserId = result.Result.Id,
            BookIds = new List<Guid>
            {
                createBookResult.Result.Id
            }
        };
        var addBookRequest = new HttpRequestMessage(HttpMethod.Post, $"{Endpoint}/{result.Result.Id}/books");
        addBookRequest.SetContent(addBookToUserComand);
        var addBookResponse = await client.SendAsync(addBookRequest);
        Assert.Equal(HttpStatusCode.OK, addBookResponse.StatusCode);
        var addBookResult = await addBookResponse.GetContentAsync<MethodResult<UserView>>();
        Assert.NotNull(addBookResult);
        Assert.NotNull(addBookResult.Result);
        var readBook = addBookResult.Result.BookViews.FirstOrDefault();
        Assert.NotNull(readBook);
        Assert.Equal(readBook.Name, createBookResult.Result.Name);
    }

}