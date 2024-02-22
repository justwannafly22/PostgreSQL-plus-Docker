using Serilog;
using System.Net;
using WebAggregator.Boundary;
using WebAggregator.Infrastructure.Exceptions;

namespace WebAggregator.Infrastructure.Middleware;

public class ExceptionHandler(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ArgumentNullException ex)
        {
            await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (ArgumentException ex)
        {
            await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (NotFoundException ex)
        {
            await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.InternalServerError);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, string message, HttpStatusCode statusCode)
    {
        Log.Error($"Error occured - {message}");

        var response = httpContext.Response;

        response.ContentType = "application/json";
        response.StatusCode = (int)statusCode;

        await response.WriteAsync(new BaseResponseModel
        {
            Message = message,
            StatusCode = statusCode
        }.ToString());
    }
}
