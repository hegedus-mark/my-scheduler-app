using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Context;

public class DbContextFactory : IDesignTimeDbContextFactory<DbContext>
{
    public DbContext CreateDbContext(string[] args)
    {

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);

        return new DbContext(optionsBuilder.Options);

    }
}