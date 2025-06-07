using DevLoopLB.Exceptions;
using System.Text.Json;

namespace DevLoopLB.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, message) = exception switch
            {
                EntityNotFoundException => (StatusCodes.Status404NotFound, exception.Message),
                BusinessValidationException => (StatusCodes.Status400BadRequest, exception.Message),
                //DuplicateEntityException => (StatusCodes.Status409Conflict, exception.Message),
                //FileStorageException => (StatusCodes.Status500InternalServerError, "File operation failed"),
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
                ArgumentException => (StatusCodes.Status400BadRequest, "Invalid request data"),
                _ => (StatusCodes.Status500InternalServerError, "An internal server error occurred")
            };

            context.Response.StatusCode = statusCode;

            var response = new
            {
                error = new
                {
                    message = message,
                    statusCode = statusCode,
                    timestamp = DateTime.UtcNow
                }
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
