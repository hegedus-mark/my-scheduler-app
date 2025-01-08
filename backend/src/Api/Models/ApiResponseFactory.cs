using Application.Shared.Results;
using IResult = Application.Shared.Results.IResult;

namespace Api.Models;

public static class ApiResponseFactory
{
    public static ApiResponseBase CreateFromResult(IResult result)
    {
        if (result.IsFailure)
            throw new ArgumentException(
                "Failed results should be handled by the problem details converter",
                nameof(result)
            );

        var resultType = result.GetType();

        if (resultType == typeof(Result))
            return new EmptyApiResponse();

        if (!resultType.IsGenericType)
            throw new ArgumentException(
                $"Unexpected non-generic result type: {resultType.Name}",
                nameof(result)
            );

        var genericTypeDefinition = resultType.GetGenericTypeDefinition();

        var converter = CreateConverter(genericTypeDefinition);

        return converter.Convert(result);
    }

    private static IResultConverter CreateConverter(Type genericTypeDefinition)
    {
        if (genericTypeDefinition == typeof(Result<>))
            return new ValueResultConverter();

        if (genericTypeDefinition == typeof(CollectionResult<>))
            return new CollectionQueryResultConverter();

        throw new ArgumentException($"Unsupported result type: {genericTypeDefinition.Name}");
    }
}
