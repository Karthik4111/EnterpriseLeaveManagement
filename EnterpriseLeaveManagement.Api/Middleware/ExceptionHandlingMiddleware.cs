using EnterpriseLeaveManagement.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using FluentValidationException = FluentValidation.ValidationException;

namespace EnterpriseLeaveManagement.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next,ILogger<ExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex,"Unhandled exception occurred while processing {Path}",context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
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
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}