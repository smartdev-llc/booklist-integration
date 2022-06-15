using System.Net;
using Microsoft.AspNetCore.Mvc;
using TLM.Books.Application.Features.BookFeature.Commands;
using TLM.Books.Application.Features.BookFeature.Queries;
using TLM.Books.Application.Models;
using TLM.Books.Common;
using TLM.Books.Common.Error;

namespace TLM.Books.API.Controllers;

public class BooksController: BaseApiController
{
    /// <summary>
    /// Creates a New Book.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(MethodResult<BookView>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create(CreateBookCommand command)
    {
        try
        {
            var result = await Mediator.Send(command);
            return result.GetActionResult();
        }
        catch (Exception ex)
        {
            var errorCommandResult = new VoidMethodResult();
            errorCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace);
            errorCommandResult.StatusCode = StatusCodes.Status400BadRequest;
            return Ok(errorCommandResult);
        }
        return Ok(await Mediator.Send(command));
    }
    
    /// <summary>
    /// Gets all Books.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(MethodResult<IEnumerable<BookView>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await Mediator.Send(new GetAllBooksQuery());
            return result.GetActionResult();
        }
        catch (Exception ex)
        {
            var errorCommandResult = new VoidMethodResult();
            errorCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace);
            errorCommandResult.StatusCode = StatusCodes.Status400BadRequest;
            return Ok(errorCommandResult);
        }
    }
    /// <summary>
    /// Find Book by name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(MethodResult<IEnumerable<BookView>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Search(string name)
    {
        try
        {
            var result = await Mediator.Send(new GetBookByNameQuery {Name = name});
            return result.GetActionResult();
        }
        catch (Exception ex)
        {
            var errorCommandResult = new VoidMethodResult();
            errorCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace);
            errorCommandResult.StatusCode = StatusCodes.Status400BadRequest;
            return Ok(errorCommandResult);
        }
    }
    /// <summary>
    /// Deletes Book Entity based on Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(MethodResult<BookView>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var result = await Mediator.Send(new DeleteBookCommand {Id = id});
            return result.GetActionResult();
        }
        catch (Exception ex)
        {
            var errorCommandResult = new VoidMethodResult();
            errorCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace);
            errorCommandResult.StatusCode = StatusCodes.Status400BadRequest;
            return Ok(errorCommandResult);
        }
    }
    /// <summary>
    /// Updates the Book Entity based on Id.   
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(MethodResult<BookView>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update(Guid id, UpdateBookCommand command)
    {
        try
        {
            command.Id = id;
            var result = await Mediator.Send(command);
            return result.GetActionResult();
        }
        catch (Exception ex)
        {
            var errorCommandResult = new VoidMethodResult();
            errorCommandResult.AddErrorMessage(Helpers.GetExceptionMessage(ex), ex.StackTrace);
            errorCommandResult.StatusCode = StatusCodes.Status400BadRequest;
            return Ok(errorCommandResult);
        }
    }
}