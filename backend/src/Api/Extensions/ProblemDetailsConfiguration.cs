using System.Diagnostics;

namespace Api.Extensions;

public static class ProblemDetailsConfiguration
{
    public static void ConfigureProblemDetails(this IServiceCollection service)
    {
        service.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance = context.HttpContext.Request.Path;
                context.ProblemDetails.Extensions["traceId"] =
                    Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
            };
        });
    }
}
