using Application.Shared.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();

        services.AddHandlers();

        return services;
    }

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
