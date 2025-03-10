﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkLogger.Domain.Interfaces;
using WorkLogger.Infrastructure.Persistence;
using WorkLogger.Infrastructure.Repositories;

namespace WorkLogger.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("WorkLogger");
        var testConnectionString = configuration.GetConnectionString("WorkLoggerTests");
        var scripts = configuration.GetConnectionString("Scripts");
        
        
        services.AddDbContext<WorkLoggerDbContext>(options => options.UseNpgsql(connectionString).UseLowerCaseNamingConvention());
        services.AddScoped<IWorkLoggerRepository, WorkLoggerRepository>();
      //  Migrator.Migrate(connectionString, scripts);
    }
}