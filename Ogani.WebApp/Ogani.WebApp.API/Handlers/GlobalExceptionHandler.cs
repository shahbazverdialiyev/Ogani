using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ogani.WebApp.Business.Exceptions;

namespace Ogani.Api.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Unhandled Exception: {Message}", exception.Message);

            var (statusCode, title, detail, errors) = MapException(exception, httpContext);

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path
            };

            if (errors != null)
            {
                problemDetails.Extensions["errors"] = errors;
            }

            if (_env.IsDevelopment() || httpContext.User.IsInRole("Admin"))
            {
                problemDetails.Extensions["exceptionDetails"] = new
                {
                    Message = exception.Message,
                    StackTrace = exception.StackTrace,
                    InnerException = exception.InnerException?.Message
                };
            }

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        private (int StatusCode, string Title, string Detail, IDictionary<string, string[]>? Errors) MapException(Exception exception, HttpContext context)
        {
            return exception switch
            {
                NotFoundException ex => (
                    StatusCodes.Status404NotFound,
                    "Resource Not Found",
                    ex.Message,
                    null
                ),

                BusinessValidationException ex => (
                    StatusCodes.Status400BadRequest,
                    "Validation Error",
                    "Validation failed.",
                    ex.Errors.GroupBy(e => e.PropertyName)
                             .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                ),

                BusinessException ex => (
                    StatusCodes.Status400BadRequest,
                    "Business Rule Violation",
                    ex.Message,
                    null
                ),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    "Internal Server Error",
                    (_env.IsDevelopment() || context.User.IsInRole("Admin"))
                        ? exception.Message
                        : "An unexpected error occurred. Please try again later.",
                    null
                )
            };
        }
    }
}