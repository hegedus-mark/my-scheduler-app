using Application.Calendar.Interfaces.Repositories;
using Application.Scheduling.Interfaces.Repositories;
using Infrastructure.Shared.Context;
using Infrastructure.Shared.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        //Register DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            )
        );

        // Register Unit of Work
        services.AddScoped<ICalendarUnitOfWork, SharedUnitOfWork>();
        services.AddScoped<ISchedulingUnitOfWork, SharedUnitOfWork>();

        return services;
    }
}
