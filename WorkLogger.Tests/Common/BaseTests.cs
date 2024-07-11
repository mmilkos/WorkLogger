using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WorkLogger.Domain.Exceptions;
using WorkLogger.Domain.Interfaces;
using WorkLogger.Infrastructure.Persistence;
using WorkLogger.Infrastructure.Repositories;

namespace WorkLogger.Tests.Common;

public class BaseTests : IDisposable
{
    protected WorkLoggerDbContext _dbContext;
    protected readonly IMediator _mediator;
    private IDbContextTransaction _transaction;
    
    public BaseTests()
    {
        var services = new ServiceCollection();

        var dbContextOptions = new DbContextOptionsBuilder<WorkLoggerDbContext>()
            .UseSqlServer("Server=LAPTOP-MMILKOS;Database=WorkLoggerTestsDb;Trusted_Connection=True;TrustServerCertificate=True")
            .Options;
        
        _dbContext = new WorkLoggerDbContext(dbContextOptions);
        
        services.AddScoped<IWorkLoggerRepository, WorkLoggerRepository>();
        services.AddSingleton(_ => _dbContext);
        services.AddMediatR(typeof(WorkLogger.Application._Commands.Companies.RegisterCompanyWithCeoCommandHandler));
        services.AddMediatR(typeof(WorkLogger.Application._Commands.Users.RegisterUserCommandHandler));
        
        var provider = services.BuildServiceProvider();
        _mediator = provider.GetRequiredService<IMediator>();
        
        _transaction = _dbContext.Database.BeginTransaction();
    }

    public void Dispose()
    {
        _transaction.Rollback();
        _transaction.Dispose();
        _dbContext.Dispose();
    }
}