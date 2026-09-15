using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace IMS.API.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlingMiddleware> logger)
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
            catch (ValidationException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Validation error occurred while processing {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                await HandleValidationExceptionAsync(context, ex);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Database error occurred while processing {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                await HandleDatabaseExceptionAsync(context);
            }
            catch (Exception ex)
            {
                var userId = context.User?
                .FindFirst("sub")?.Value ?? "Anonymous";

                var ipAddress = context.Connection.RemoteIpAddress?
                    .ToString() ?? "Unknown";

                _logger.LogError(
                    ex,
                    "Unhandled exception | {Method} {Path} | UserId: {UserId} | IP: {IpAddress}",
                    context.Request.Method,
                    context.Request.Path,
                    userId,
                    ipAddress);

                await HandleExceptionAsync(context);
            }
        }

        private static async Task HandleValidationExceptionAsync(
            HttpContext context,
            ValidationException exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var errors = exception.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(e => e.ErrorMessage).ToArray());

            var response = new
            {
                success = false,
                message = "Validation failed.",
                errors = errors
            };

            await context.Response.WriteAsJsonAsync(response);
        }

        private static async Task HandleDatabaseExceptionAsync(
            HttpContext context)
        {
            context.Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            context.Response.ContentType = "application/json";

            var response = new
            {
                success = false,
                message = "A database error occurred while processing your request."
            };

            await context.Response.WriteAsJsonAsync(response);
        }

        private static async Task HandleExceptionAsync(
            HttpContext context)
        {
            context.Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            context.Response.ContentType = "application/json";

            var response = new
            {
                success = false,
                message = "An unexpected error occurred."
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
