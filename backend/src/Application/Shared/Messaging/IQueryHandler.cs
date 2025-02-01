namespace Application.Shared.Messaging;

/// <summary>
///     Defines a handler for queries.
/// </summary>
/// <typeparam name="TQuery">The type of query to handle</typeparam>
/// <typeparam name="TResult">The type of result returned</typeparam>
/// <remarks>
///     Query handlers should focus on retrieving and transforming data
///     without modifying system state.
/// </remarks>
public interface IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    /// <summary>
    ///     Handles the specified query and returns the requested data.
    /// </summary>
    /// <param name="query">The query to handle</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>The requested data</returns>
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
