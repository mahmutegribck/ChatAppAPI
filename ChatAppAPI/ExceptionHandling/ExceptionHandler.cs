using ChatAppAPI.ExceptionHandling.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ChatAppAPI.ExceptionHandling
{
    public class ExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is null) return false;

            if (exception is NotFoundException)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Type = exception.GetType().Name,
                    Title = "Not Found Error",
                    Detail = exception.Message,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"

                };
                await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
                return true;

            }
            else if (exception is ArgumentException)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Type = exception.GetType().Name,
                    Title = "Bad Request Error",
                    Detail = exception.Message,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"

                };
                await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

                return true;
            }
            else
            {
                var problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = exception.GetType().Name,
                    Title = "Internal Server Error",
                    Detail = exception.Message,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"

                };
                await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

                return true;
            }
        }
    }
}
