using System.Collections.Concurrent;
using System.Reflection;
using Application.Shared.Exceptions;
using Application.Shared.Results;
using Domain.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.Shared.Messaging;

public class Mediator : IMediator
{
    // Caches for handler types and methods
    private static readonly ConcurrentDictionary<Type, Type> HandlerTypeCache = new();
    private static readonly ConcurrentDictionary<Type, MethodInfo> HandleMethodCache = new();

    private readonly ILogger<Mediator> _logger;
    private readonly IServiceProvider _provider;

    public Mediator(IServiceProvider provider, ILogger<Mediator> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public async Task<Result> SendAsync(
        ICommand command,
        CancellationToken cancellationToken = default
    )
    {
        var commandType = command.GetType();

        var handlerType = HandlerTypeCache.GetOrAdd(
            commandType,
            type => typeof(ICommandHandler<>).MakeGenericType(type)
        );

        var handler = _provider.GetService(handlerType);
        if (handler == null)
            throw new MissingHandlerException(handlerType.Name);

        var handleMethod = HandleMethodCache.GetOrAdd(
            handlerType,
            type => type.GetMethod(nameof(ICommandHandler<ICommand>.HandleAsync))!
        );

        return await (Task<Result>)
            handleMethod.Invoke(handler, new object?[] { command, cancellationToken })!;
    }

    public async Task<Result<TResult>> SendAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default
    )
    {
        var commandType = command.GetType();

        var handlerType = HandlerTypeCache.GetOrAdd(
            commandType,
            type => typeof(ICommandHandler<,>).MakeGenericType(type, typeof(TResult))
        );

        var handler = _provider.GetService(handlerType);
        if (handler == null)
            throw new MissingHandlerException(handlerType.Name);

        var handleMethod = HandleMethodCache.GetOrAdd(
            handlerType,
            type => type.GetMethod(nameof(ICommandHandler<ICommand<TResult>, TResult>.HandleAsync))!
        );

        return await (Task<Result<TResult>>)
            handleMethod.Invoke(handler, new object[] { command, cancellationToken })!;
    }

    public async Task<TResult> SendAsync<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken = default
    )
    {
        var queryType = query.GetType();

        var handlerType = HandlerTypeCache.GetOrAdd(
            queryType,
            type => typeof(IQueryHandler<,>).MakeGenericType(type, typeof(TResult))
        );

        var handler = _provider.GetService(handlerType);
        if (handler == null)
            throw new MissingHandlerException(handlerType.Name);

        var handleMethod = HandleMethodCache.GetOrAdd(
            handlerType,
            type => type.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync))!
        );

        return await (Task<TResult>)
            handleMethod.Invoke(handler, new object[] { query, cancellationToken })!;
    }

    public async Task PublishAsync(IDomainEvent domainEvent)
    {
        var eventType = domainEvent.GetType();

        var handlerType = HandlerTypeCache.GetOrAdd(
            eventType,
            type => typeof(IDomainEventHandler<>).MakeGenericType(type)
        );

        var handlers = _provider.GetServices(handlerType).ToArray();
        if (handlers.Length == 0)
        {
            _logger.LogWarning("No handlers registered for domain event: {EventType}", eventType);
            return;
        }

        var handleMethod = HandleMethodCache.GetOrAdd(
            handlerType,
            type => type.GetMethod(nameof(IDomainEventHandler<IDomainEvent>.HandleAsync))!
        );

        foreach (var handler in handlers)
            await (Task)
                handleMethod.Invoke(handler, new object[] { domainEvent, CancellationToken.None })!;
    }
}
