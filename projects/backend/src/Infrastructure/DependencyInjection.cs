using Application.Calendar.Interfaces.Repositories;
using Application.Scheduling.Interfaces.Repositories;
using Infrastructure.Shared.Context;
using Infrastructure.Shared.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

/// <summary>
///     Provides extension methods for configuring infrastructure services in the dependency injection container.
///     This class centralizes all infrastructure-related service registrations.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString
    )
    {
        //Register DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            )
        );

        // Register Unit of Work
        services.AddScoped<ICalendarUnitOfWork, SharedUnitOfWork>();
        services.AddScoped<ISchedulingUnitOfWork, SharedUnitOfWork>();

        return services;
    }
}
