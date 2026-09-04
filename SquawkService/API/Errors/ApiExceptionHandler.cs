using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ParrotInc.SquawkService.Domain.Exceptions;

namespace ParrotInc.SquawkService.Api;

public sealed class ApiExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<ApiExceptionHandler> _logger;

    public ApiExceptionHandler(
        IProblemDetailsService problemDetailsService,
        ILogger<ApiExceptionHandler> logger)
    {
        _problemDetailsService = problemDetailsService;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not SquawkRuleViolationException ruleViolation)
        {
            _logger.LogError(exception, "Unhandled exception while processing the request.");
            return false;
        }

        var statusCode = ruleViolation.Code switch
        {
            "duplicate_squawk" => StatusCodes.Status409Conflict,
            "posting_too_fast" => StatusCodes.Status429TooManyRequests,
            _ => StatusCodes.Status400BadRequest
        };

        if (statusCode == StatusCodes.Status429TooManyRequests)
        {
            httpContext.Response.Headers.RetryAfter = "20";
        }

        httpContext.Response.StatusCode = statusCode;

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = "Squawk business rule violation",
                Detail = ruleViolation.Message,
                Type = $"https://httpstatuses.com/{statusCode}",
                Extensions =
                {
                    ["code"] = ruleViolation.Code
                }
            }
        });
    }
}
