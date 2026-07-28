using EnterpriseLeaveManagement.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using FluentValidationException = FluentValidation.ValidationException;

namespace EnterpriseLeaveManagement.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (ex is UnauthorizedException)
            {
                _logger.LogWarning(
                    "Unauthorized request for {Path}: {Message}",
                    context.Request.Path,
                    ex.Message);
            }
            else
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception occurred while processing {Path}",
                    context.Request.Path);
            }

            await HandleExceptionAsync(context, ex, _environment.IsDevelopment());
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception,
        bool includeDetails)
    {
        var statusCode = StatusCodes.Status500InternalServerError;
        var title = "Internal Server Error";

        switch (exception)
        {
            case FluentValidationException:
                statusCode = StatusCodes.Status400BadRequest;
                title = "Validation Error";
                break;

            case BadRequestException:
                statusCode = StatusCodes.Status400BadRequest;
                title = "Bad Request";
                break;

            case BusinessException:
                statusCode = StatusCodes.Status409Conflict;
                title = "Business Rule Violation";
                break;

            case NotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                title = "Resource Not Found";
                break;

            case UnauthorizedException:
                statusCode = StatusCodes.Status401Unauthorized;
                title = "Unauthorized";
                break;
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = statusCode == StatusCodes.Status500InternalServerError && !includeDetails
                ? "An unexpected error occurred."
                : exception.Message,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}