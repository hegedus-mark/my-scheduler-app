using Application.Shared.Results;

namespace Application.Shared.Messaging;

public interface IMediator
{
    Task<Result<TResult>> SendAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default
    );

    Task<Result> SendAsync(ICommand command, CancellationToken cancellationToken = default);

    Task<TResult> SendAsync<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken = default
    );
}
