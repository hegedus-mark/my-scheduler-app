using Application.Shared.Results;

namespace Application.Shared.Messaging;

/// <summary>
///     Interface for queries that retrieve data from the system.
/// </summary>
/// <typeparam name="TResult">The type of data to retrieve</typeparam>
/// <remarks>
///     Queries should:
///     - Be named to describe the data being retrieved (e.g., GetUserById, ListActiveOrders)
///     - Not modify system state
/// </remarks>
public interface IQuery<TResult> { }

public interface ISingleQuery<TResult> : IQuery<Result<TResult>> { }

public interface ICollectionQuery<TResult> : IQuery<CollectionResult<TResult>> { }
