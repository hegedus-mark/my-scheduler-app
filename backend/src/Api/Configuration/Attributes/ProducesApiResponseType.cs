using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Configuration.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ProducesApiResponseType : ProducesResponseTypeAttribute
{
    public ProducesApiResponseType(int statusCode)
        : base(typeof(EmptyApiResponse), statusCode) { }

    public ProducesApiResponseType(Type type, int statusCode)
        : base(GetWrappedType(type), statusCode) { }

    private static Type GetWrappedType(Type responseType)
    {
        if (IsApiResponseType(responseType))
            return responseType;

        if (IsEnumerableType(responseType))
        {
            //This only works if it has 1 type
            var elementType = GetEnumerableElementType(responseType);
            return typeof(CollectionApiResponse<>).MakeGenericType(elementType);
        }

        return typeof(ApiResponse<>).MakeGenericType(responseType);
    }

    private static bool IsApiResponseType(Type type)
    {
        return type == typeof(ApiResponseBase);
    }

    private static bool IsEnumerableType(Type type)
    {
        return type != typeof(string) && typeof(IEnumerable<>).IsAssignableFrom(type);
    }

    private static Type GetEnumerableElementType(Type type)
    {
        return type.GetGenericArguments()[0];
    }
}
