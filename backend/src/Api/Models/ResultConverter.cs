using Application.Shared.Results;
using IResult = Application.Shared.Results.IResult;

namespace Api.Models;

public interface IResultConverter
{
    ApiResponseBase Convert(IResult result);
}

// Converter for ValueResult<T>
public class ValueResultConverter : IResultConverter
{
    public ApiResponseBase Convert(IResult result)
    {
        var resultType = result.GetType();
        var valueProperty = resultType.GetProperty(nameof(IValueResult<object>.Value))!;
        var value = valueProperty.GetValue(result);

        var genericArgument = resultType.GetGenericArguments()[0];
        var apiResponseType = typeof(ApiResponse<>).MakeGenericType(genericArgument);

        return (ApiResponseBase)Activator.CreateInstance(apiResponseType, value)!;
    }
}

// Converter for CollectionQueryResult<T>
public class CollectionQueryResultConverter : IResultConverter
{
    public ApiResponseBase Convert(IResult result)
    {
        var resultType = result.GetType();
        var itemsProperty = resultType.GetProperty(nameof(ICollectionResult<object>.Items))!;
        var countProperty = resultType.GetProperty(nameof(ICollectionResult<object>.Count))!;

        var items = itemsProperty.GetValue(result);
        var count = (int)countProperty.GetValue(result)!;

        var metadata = new CollectionMetadata
        {
            TotalCount = count,
            HasNextPage = false,
            CurrentPage = 1,
            PageSize = count,
        };

        var genericArgument = resultType.GetGenericArguments()[0];
        var apiResponseType = typeof(CollectionApiResponse<>).MakeGenericType(genericArgument);

        return (ApiResponseBase)Activator.CreateInstance(apiResponseType, items, metadata)!;
    }
}
