using System.Text.Json;
using StageSim.Domain.Exceptions;

namespace StageSim.API.Middlewares;

public class GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        logger.LogError(exception, "Unhandled exception: {message}", exception.Message);

        if (exception is IDomainException domainException)
            return WriteResponse(context, domainException.StatusCode, exception.Message);

        return WriteResponse(context, StatusCodes.Status500InternalServerError);
    }

    private static Task WriteResponse(HttpContext context, int statusCode, string message = "An unexpected error occurred.")
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(JsonSerializer.Serialize(new { error = message }));
    }
}
