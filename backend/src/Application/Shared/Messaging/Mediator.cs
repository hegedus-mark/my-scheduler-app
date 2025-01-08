using Application.Shared.Exceptions;
using Application.Shared.Results;

namespace Application.Shared.Messaging;

public class Mediator : IMediator
{
    private readonly IServiceProvider _provider;

    public Mediator(IServiceProvider provider)
    {
        _provider = provider;
    }

    // Handles commands without return value
    public async Task<Result> SendAsync(
        ICommand command,
        CancellationToken cancellationToken = default
    )
    {
        // Get the concrete command type
        var commandType = command.GetType();

        // Construct handler type for void commands
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);

        var handler = _provider.GetService(handlerType);
        if (handler == null)
            throw new MissingHandlerException(handlerType.Name);

        var handleMethod = handlerType.GetMethod(nameof(ICommandHandler<ICommand>.HandleAsync));

        return await (Task<Result>)
            handleMethod!.Invoke(handler, new object?[] { command, cancellationToken })!;
    }

    // Handles commands with return value
    public async Task<Result<TResult>> SendAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default
    )
    {
        var commandType = command.GetType();

        // Construct handler type for commands with results
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResult));

        var handler = _provider.GetService(handlerType);
        if (handler == null)
            throw new MissingHandlerException(handlerType.Name);

        var handleMethod = handlerType.GetMethod(
            nameof(ICommandHandler<ICommand<TResult>, TResult>.HandleAsync)
        );

        return await (Task<Result<TResult>>)
            handleMethod!.Invoke(handler, new object[] { command, cancellationToken })!;
    }

    // Handles queries
    public async Task<TResult> SendAsync<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken = default
    )
    {
        var queryType = query.GetType();

        // Construct handler type for queries
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResult));

        var handler = _provider.GetService(handlerType);
        if (handler == null)
            throw new MissingHandlerException(handlerType.Name);

        var handleMethod = handlerType.GetMethod(
            nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync)
        );

        return await (Task<TResult>)
            handleMethod!.Invoke(handler, new object[] { query, cancellationToken })!;
    }
}
