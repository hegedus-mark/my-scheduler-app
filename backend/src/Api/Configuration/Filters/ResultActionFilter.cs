using Api.Configuration.Attributes;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using IResult = Application.Shared.Results.IResult;
using ResultStatus = Application.Shared.Results.ResultStatus;

namespace Api.Configuration.Filters;

public class ResultActionFilter : IActionFilter
{
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    public ResultActionFilter(ProblemDetailsFactory problemDetailsFactory)
    {
        _problemDetailsFactory = problemDetailsFactory;
    }

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is not ObjectResult objectResult)
            return;
        if (objectResult.Value is not IResult result)
            return;

        if (result.IsFailure)
        {
            HandleErrorResult(context, result);
        }
        else
        {
            var response = ApiResponseFactory.CreateFromResult(result);

            context.Result = new ObjectResult(response) { StatusCode = (int)result.Status };
        }
    }

    private void HandleErrorResult(ActionExecutedContext context, IResult result)
    {
        var expectedResults = context
            .ActionDescriptor.EndpointMetadata.OfType<ExpectedResultsAttribute>()
            .FirstOrDefault();

        var status =
            expectedResults?.AllowedStatuses.Contains(result.Error!.Status) ?? false
                ? (int)result.Error.Status
                : (int)ResultStatus.InternalError;

        var problemDetails = _problemDetailsFactory.CreateProblemDetails(
            context.HttpContext,
            status,
            result.Error!.Code,
            detail: result.Error.Message
        );

        context.Result = new ObjectResult(problemDetails) { StatusCode = status };
    }
}
