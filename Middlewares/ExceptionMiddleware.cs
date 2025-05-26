using System.Net;
using booking_service.Exceptions;

namespace booking_service.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // go to next middleware or endpoint
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        int statusCode;

        switch (ex)
        {
            case NotFoundException:
                statusCode = (int)HttpStatusCode.NotFound;
                break;
            case InvalidArgumentException:
                statusCode = (int)HttpStatusCode.BadRequest;
                break;

            default:
                statusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        var result = new
        {
            error = ex.Message,
            statusCode
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsJsonAsync(result);
    }
}