using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using TLM.Books.Application.Features.BookFeature.Commands;
using TLM.Books.Application.Features.BookFeature.Queries;
using TLM.Books.Application.Models;
using TLM.Books.Common.Error;
using TLM.Books.IntegrationTests.Configurations;
using Xunit;

namespace TLM.Books.IntegrationTests.Scenarios.Books;

[Collection(nameof(BookCollectionFixtureDefinition))]
public class BookTests
{
    private readonly BookApplicationFactory _factory;
    private string Endpoint => "api/books";
    
    public BookTests(BookApplicationFactory factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task QueryBook_GetAllBooks_ShouldBeSuccess()
    {
        var client = _factory.CreateDefaultClient();
        
        var response = await client.GetAsync(Endpoint);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.GetContentAsync<MethodResult<IEnumerable<BookView>>>();
        Assert.NotNull(result);
        Assert.True(result.IsOK);
        Assert.NotNull(result.Result);
        Assert.NotEmpty(result.Result);
    }
    
    [Fact]
    public async Task CreateBook_ValidData_ShouldBeSuccess()
    {
        var client = _factory.CreateDefaultClient();

        var createBookCommand = new CreateBookCommand
        {
            Author = "John Cena",
            Name = "You cannot see me",
            ISBN = "WWE"
        };
        var request = new HttpRequestMessage(HttpMethod.Post, Endpoint);
        request.SetContent(createBookCommand);
        var response = await client.SendAsync(request);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.GetContentAsync<MethodResult<BookView>>();
        Assert.NotNull(result);
        Assert.True(result.IsOK);
        Assert.NotNull(result.Result);
        Assert.Equal(createBookCommand.Author, result.Result.Author);
    }
    
    [Fact]
    public async Task UpdateBook_ValidData_ShouldBeSuccess()
    {
        var client = _factory.CreateDefaultClient();

        var createBookCommand = new CreateBookCommand
        {
            Author = "John Wick",
            Name = "Do not mess with my dog's wife",
            ISBN = "Test"
        };
        var request = new HttpRequestMessage(HttpMethod.Post, Endpoint);
        request.SetContent(createBookCommand);
        var response = await client.SendAsync(request);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.GetContentAsync<MethodResult<BookView>>();
        Assert.NotNull(result);
        Assert.True(result.IsOK);
        Assert.NotNull(result.Result);
        Assert.Equal(createBookCommand.Author, result.Result.Author);

        var updateBookCommand = new UpdateBookCommand
        {
            ISBN = "Google"
        };
        var updateRequest = new HttpRequestMessage(HttpMethod.Put, $"{Endpoint}/{result.Result.Id}");
        updateRequest.SetContent(updateBookCommand);
        var updateResponse = await client.SendAsync(updateRequest);
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
        
        var updateResult = await updateResponse.GetContentAsync<MethodResult<BookView>>();
        Assert.NotEmpty(updateResult.Result.ISBN);
        Assert.Equal(updateBookCommand.ISBN, updateResult.Result.ISBN);
    }
    
    [Fact]
    public async Task DeleteBook_ValidData_ShouldBeSuccess()
    {
        var client = _factory.CreateDefaultClient();

        var createBookCommand = new CreateBookCommand
        {
            Author = "John Wick",
            Name = "Test deleted",
            ISBN = "Test"
        };
        var request = new HttpRequestMessage(HttpMethod.Post, Endpoint);
        request.SetContent(createBookCommand);
        var response = await client.SendAsync(request);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.GetContentAsync<MethodResult<BookView>>();
        Assert.NotNull(result);
        Assert.True(result.IsOK);
        Assert.NotNull(result.Result);
        Assert.Equal(createBookCommand.Author, result.Result.Author);

        var deleteBookCommand = new DeleteBookCommand();
        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"{Endpoint}/{result.Result.Id}");
        deleteRequest.SetContent(deleteBookCommand);
        var deleteResponse = await client.SendAsync(deleteRequest);
        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        
        var deleteResult = await deleteResponse.GetContentAsync<MethodResult<BookView>>();
        Assert.NotNull(deleteResult);
        Assert.Equal(result.Result.Id, deleteResult.Result.Id);
    }
    
    [Fact]
    public async Task SearchBook_ValidData_ShouldBeSuccess()
    {
        var client = _factory.CreateDefaultClient();

        var createBookCommand = new CreateBookCommand
        {
            Author = "John Wick",
            Name = "Wish you can FIND me",
            ISBN = "Test"
        };
        var request = new HttpRequestMessage(HttpMethod.Post, Endpoint);
        request.SetContent(createBookCommand);
        var response = await client.SendAsync(request);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.GetContentAsync<MethodResult<BookView>>();
        Assert.NotNull(result);
        Assert.True(result.IsOK);
        Assert.NotNull(result.Result);
        Assert.Equal(createBookCommand.Author, result.Result.Author);

        var searchResponse = await client.GetAsync($"{Endpoint}/search?name=find");
        Assert.Equal(HttpStatusCode.OK, searchResponse.StatusCode);
        
        var searchResult = await searchResponse.GetContentAsync<MethodResult<IEnumerable<BookView>>>();
        Assert.NotNull(searchResult);
        Assert.NotNull(searchResult.Result);
        Assert.NotEmpty(searchResult.Result);
    }
}