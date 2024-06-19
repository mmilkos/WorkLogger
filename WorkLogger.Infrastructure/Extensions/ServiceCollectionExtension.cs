using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkLogger.Infrastructure.Persistence;

namespace WorkLogger.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("WorkLogger");
        services.AddDbContext<WorkLoggerDbContext>(options => options.UseSqlServer(connectionString));
        Migrator.Migrate(connectionString);
    }
}