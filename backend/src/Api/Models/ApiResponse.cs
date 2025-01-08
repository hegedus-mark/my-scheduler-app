namespace Api.Models;

public abstract record ApiResponseBase(bool IsSuccess = true);

public sealed record EmptyApiResponse : ApiResponseBase;

// For single-value responses
public sealed record ApiResponse<TResponse>(TResponse Data) : ApiResponseBase { }

// For collection responses
public sealed record CollectionApiResponse<TResponse>(
    IEnumerable<TResponse> Data,
    CollectionMetadata Metadata
) : ApiResponseBase;

// Metadata for collections
public sealed class CollectionMetadata
{
    public int TotalCount { get; init; }
    public bool HasNextPage { get; init; }
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }
}
