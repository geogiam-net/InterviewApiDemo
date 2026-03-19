using Demo.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Exceptions;

// https://www.milanjovanovic.tech/blog/global-error-handling-in-aspnetcore-from-middleware-to-modern-handlers
internal sealed class ValidationExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ValidationException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        var context = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Detail = "One or more validation errors occurred",
                Status = StatusCodes.Status400BadRequest,
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
            }
        };

        context.ProblemDetails.Extensions.Add("errors", ((ValidationException)exception).Errors);

        return await problemDetailsService.TryWriteAsync(context);
    }
}
