using System.Net;
using System.Text.Json;
using TLM.Books.Common.Error;
using TLM.Books.Domain.Commons;

namespace TLM.Books.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var errorCommandResult = new VoidMethodResult();
        errorCommandResult.AddErrorMessage("Message: " + ex.Message + ", InnerMessage: " + ex.InnerException?.Message,
            ex.StackTrace);
        errorCommandResult.StatusCode = StatusCodes.Status400BadRequest;
        var result = JsonSerializer.Serialize(errorCommandResult);
        await context.Response.WriteAsync(result);
        // await context.Response.WriteAsync(new ErrorDetails()
        // {
        //     StatusCode = context.Response.StatusCode,
        //     Message = ex.Message +", "+ex.StackTrace
        // }.ToString());
    }
}