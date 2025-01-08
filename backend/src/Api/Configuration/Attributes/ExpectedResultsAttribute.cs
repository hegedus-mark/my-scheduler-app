using Application.Shared.Results;

namespace Api.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class ExpectedResultsAttribute : Attribute
{
    public ExpectedResultsAttribute(params ResultStatus[] allowedStatuses)
    {
        AllowedStatuses = allowedStatuses;
    }

    public ResultStatus[] AllowedStatuses { get; }
}
