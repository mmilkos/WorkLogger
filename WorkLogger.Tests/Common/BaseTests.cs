using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkLogger.Domain.Exceptions;
using WorkLogger.Domain.Interfaces;
using WorkLogger.Infrastructure.Persistence;
using WorkLogger.Infrastructure.Repositories;

namespace WorkLogger.Tests.Common;

public class BaseTests : IDisposable
{
    protected readonly WorkLoggerDbContext _dbContext;
    protected readonly IMediator _mediator;
    
    public BaseTests()
    {
        var services = new ServiceCollection();

        var dbContextOptions = new DbContextOptionsBuilder<WorkLoggerDbContext>()
            .UseSqlServer("Server=LAPTOP-MMILKOS;Database=WorkLoggerTestsDb;Trusted_Connection=True;TrustServerCertificate=True")
            .Options;
        
        _dbContext = new WorkLoggerDbContext(dbContextOptions);
        
        services.AddScoped<IWorkLoggerRepository, WorkLoggerRepository>();
        services.AddSingleton(_ => _dbContext);
        services.AddMediatR(typeof(WorkLogger.Application._Commands.Companies.RegisterCompanyCommandHandler));
        services.AddMediatR(typeof(WorkLogger.Application._Commands.Users.RegisterUserCommandHandler));
        
        var provider = services.BuildServiceProvider();
        _mediator = provider.GetRequiredService<IMediator>();

        cleanDatabase();
    }

    public void Dispose()
    {
        cleanDatabase();
     
        var anyTasks = _dbContext.UserTasks.Any();
        var anyUsers = _dbContext.Users.Any();
        var anyCompanies = _dbContext.Companies.Any();

        if (anyTasks || anyUsers || anyCompanies) throw new DataBaseDisposeFailureException();
    }

    private void cleanDatabase()
    {
        _dbContext.Database.ExecuteSqlRaw("DELETE FROM UserTasks");
        _dbContext.Database.ExecuteSqlRaw("DELETE FROM Users");
        _dbContext.Database.ExecuteSqlRaw("DELETE FROM Companies");
        _dbContext.SaveChanges();
    }
}