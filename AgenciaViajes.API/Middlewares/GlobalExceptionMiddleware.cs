using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AgenciaViajes.API.Middlewares
{
    public class GlobalExceptionMiddleware(
        IProblemDetailsService problemDetailsService,
        ILogger<GlobalExceptionMiddleware> logger
    ) : IExceptionHandler
    {

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            logger.LogError(exception, "Unhandled exception ocurred");

            httpContext.Response.StatusCode = exception switch
            {
                KeyNotFoundException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new ProblemDetails
                {
                    Type = exception.GetType().Name,
                    Title = "Se produjo un error.",
                    Detail = exception.Message
                }
            });
        }
    }
}
