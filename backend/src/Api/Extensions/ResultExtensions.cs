using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SharedKernel.Results;

namespace Api.Extensions;

public static class ResultExtensions
{
    public static int ToHttpStatusCode(this ResultStatus status)
    {
        return (int)status;
    }

    public static IActionResult ToProblemDetails<T>(this Result<T> result, HttpContext httpContext)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException(
                "Cannot convert successful result to problem details"
            );

        var statusCode = result.Status.ToHttpStatusCode();
        var problemDetailsFactory =
            httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

        var problemDetails = problemDetailsFactory.CreateProblemDetails(
            httpContext,
            statusCode,
            detail: result.Errors.FirstOrDefault()?.Message
        );

        if (problemDetails.Extensions is null)
            problemDetails.Extensions = new Dictionary<string, object?>();

        problemDetails.Extensions["errors"] = result.Errors.Select(e => new { e.Code, e.Message });

        return new ObjectResult(problemDetails) { StatusCode = statusCode };
    }
}
