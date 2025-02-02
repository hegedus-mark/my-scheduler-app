using Application.Shared.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

/// <summary>
///     Provides extension methods for configuring application layer services in the dependency injection container.
///     Handles registration of the mediator pattern and automatic discovery of CQRS handlers.
/// </summary>
/// <remarks>
///     <para>
///         This class configures:
///         1. The mediator service for implementing CQRS pattern
///         2. Automatic registration of command and query handlers
///     </para>
///     <para>
///         Handler Types Supported:
///         - ICommandHandler{TCommand, TResult} - Command handlers with return values
///         - ICommandHandler{TCommand} - Command handlers without return values
///         - IQueryHandler{TQuery, TResult} - Query handlers
///     </para>
/// </remarks>
public static class DependencyInjection
{
    /// <summary>
    ///     Adds application layer services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection for method chaining.</returns>
    /// <remarks>
    ///     This method:
    ///     1. Registers the mediator implementation
    ///     2. Automatically discovers and registers all handlers in the assembly
    /// </remarks>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();

        services.AddHandlers();

        return services;
    }

    /// <summary>
    ///     Discovers and registers all command and query handlers in the assembly.
    /// </summary>
    /// <param name="services">The service collection to add handlers to.</param>
    /// <returns>The service collection for method chaining.</returns>
    /// <remarks>
    ///     <para>
    ///         This method uses reflection to:
    ///         1. Find all non-abstract classes in the assembly
    ///         2. Identify classes that implement handler interfaces
    ///         3. Register them with their corresponding interfaces
    ///     </para>
    ///     <para>
    ///         Handler Registration Rules:
    ///         - Handlers must be concrete classes (not abstract or interfaces)
    ///         - Must implement one of the supported handler interfaces
    ///         - Are registered with scoped lifetime
    ///         - Each handler is registered with its specific interface type
    ///     </para>
    ///     <para>
    ///         Example Handler Class:
    ///         <code>
    /// public class CreateOrderHandler : ICommandHandler&lt;CreateOrderCommand, OrderDto&gt;
    /// {
    ///     public async Task&lt;Result&lt;OrderDto&gt;&gt; HandleAsync(CreateOrderCommand command)
    ///     {
    ///         // Handler implementation
    ///     }
    /// }
    /// </code>
    ///     </para>
    /// </remarks>
    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        var handlerTypes = assembly
            .GetTypes()
            .Where(t =>
                t is { IsAbstract: false, IsInterface: false }
                && t.GetInterfaces()
                    .Any(i =>
                        i.IsGenericType
                        && (
                            i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)
                            || i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)
                            || i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)
                        )
                    )
            );

        foreach (var handlerType in handlerTypes)
        {
            var handlerInterface = handlerType
                .GetInterfaces()
                .First(i =>
                    i.IsGenericType
                    && (
                        i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)
                        || i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)
                        || i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)
                    )
                );

            services.AddScoped(handlerInterface, handlerType);
        }

        return services;
    }
}
