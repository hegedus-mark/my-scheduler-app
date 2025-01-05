using SharedKernel.Results;

namespace Application.Shared.Messaging;

public interface ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<Result<TResult>> HandleAsync(
        TCommand command,
        CancellationToken cancellationToken = default
    );
}

public interface ICommandHandler<TCommand>
    where TCommand : ICommand
{
    Task<Result> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}
