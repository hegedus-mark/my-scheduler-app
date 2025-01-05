using Api.Exceptions;
using Application.Shared.Messaging;
using SharedKernel.Results;

namespace Api.Messaging;

public class Mediator : IMediator
{
    private readonly IServiceProvider _provider;

    public Mediator(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task<Result<TResult>> SendAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default
    )
    {
        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResult));

        var handler = _provider.GetService(handlerType);

        if (handler == null)
            throw new MissingServiceException(handlerType.Name);

        var method = handlerType.GetMethod(
            nameof(ICommandHandler<ICommand<TResult>, TResult>.HandleAsync)
        );

        var result = await (Task<Result<TResult>>)
            method!.Invoke(handler, new object[] { command, cancellationToken })!;

        return result;
    }
}
