using Application.Shared.Results;

namespace Application.Shared.Messaging;

public interface IQuery<TResult> { }

public interface ISingleQuery<TResult> : IQuery<Result<TResult>> { }

public interface ICollectionQuery<TResult> : IQuery<CollectionResult<TResult>> { }
