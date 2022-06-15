using System.Net;
using Microsoft.AspNetCore.Mvc;
using TLM.Books.Application.Features.BookFeature.Commands;
using TLM.Books.Application.Features.BookFeature.Queries;
using TLM.Books.Application.Features.UserFeature.Commands;
using TLM.Books.Application.Features.UserFeature.Queries;
using TLM.Books.Application.Models;
using TLM.Books.Common;
using TLM.Books.Common.Error;

namespace TLM.Books.API.Controllers;

public class UsersController: BaseApiController
{
    /// <summary>
    /// Creates a New Book.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(MethodResult<UserView>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create(CreateUserCommand command)
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
    [ProducesResponseType(typeof(MethodResult<IEnumerable<UserView>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await Mediator.Send(new GetAllUsersQuery());
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
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MethodResult<IEnumerable<UserView>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var result = await Mediator.Send(new GetUserByIdQuery() {Id = id});
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
    [ProducesResponseType(typeof(MethodResult<UserView>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var result = await Mediator.Send(new DeleteUserCommand() {Id = id});
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
    [ProducesResponseType(typeof(MethodResult<UserView>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update(Guid id, UpdateUserCommand command)
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
    
    /// <summary>
    /// Creates a New User.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("{id}/books")]
    [ProducesResponseType(typeof(MethodResult<UserView>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> AddBook(Guid id, AddBookToUserComand command)
    {
        try
        {
            command.UserId = id;
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
    
    /// <summary>
    /// Creates a New User.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpDelete("{id}/books")]
    [ProducesResponseType(typeof(MethodResult<UserView>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(VoidMethodResult), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> RemoveBook(Guid id, RemoveBookFromUserCommand command)
    {
        try
        {
            command.UserId = id;
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