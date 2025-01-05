using SharedKernel.Results;

namespace Application.Shared.Messaging;

public interface IMediator
{
    Task<Result<TResult>> SendAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default
    );
}
