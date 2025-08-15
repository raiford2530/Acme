using Acme.Shop.Application.Common.Errors;
using FluentValidation;
using System.Text.Json;

namespace Acme.Shop.Api.Middleware
{
    public class ProblemDetailsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ProblemDetailsMiddleware> _logger;

        public ProblemDetailsMiddleware(RequestDelegate next, ILogger<ProblemDetailsMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await WriteProblemAsync(ctx, ex);
            }
        }

        private static Task WriteProblemAsync(HttpContext ctx, Exception ex)
        {
            var (status, title, type) = ex switch
            {
                ValidationException => (StatusCodes.Status400BadRequest, "Validation failed", "https://http.dev/400"),
                NotFoundException => (StatusCodes.Status404NotFound, "Resource not found", "https://http.dev/404"),
                ConflictException => (StatusCodes.Status409Conflict, "Conflict", "https://http.dev/409"),
                _ => (StatusCodes.Status500InternalServerError, "Unexpected error", "https://http.dev/500")
            };

            var problem = new
            {
                type,
                title,
                status,
                detail = ex.Message,
                traceId = ctx.TraceIdentifier
            };

            ctx.Response.ContentType = "application/problem+json";
            ctx.Response.StatusCode = status;
            var json = JsonSerializer.Serialize(problem);
            return ctx.Response.WriteAsync(json);
        }
    }
}
