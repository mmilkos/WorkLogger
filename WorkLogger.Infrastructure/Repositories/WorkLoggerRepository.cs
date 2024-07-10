using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Interfaces;
using WorkLogger.Infrastructure.Persistence;

namespace WorkLogger.Infrastructure.Repositories;

public class WorkLoggerRepository : IWorkLoggerRepository
{
    private readonly WorkLoggerDbContext DbContext;
    
    public WorkLoggerRepository(WorkLoggerDbContext dbContext)
    {
        DbContext = dbContext;
    }
    
    public async Task<List<T>> GetEntitiesPaged<T>(Expression<Func<T, bool>> condition, PagedRequestDto pagingParams) where T : class
    {
        var offset = (pagingParams.Page - 1) * pagingParams.PageSize;
        var pagedResult = await DbContext.Set<T>()
            .Where(condition)
            .Skip(offset)
            .Take(pagingParams.PageSize)
            .ToListAsync();
      
        return pagedResult;
    }

    public async Task<int> GetEntitiesCount<T>(Expression<Func<T, bool>> condition) where T : class
    {
        var entities = DbContext.Set<T>().Where(condition);

        return await entities.CountAsync();
    }

    public async Task AddAsync<T>(T entity) where T: class 
    {
        await DbContext.Set<T>().AddAsync(entity);
        await DbContext.SaveChangesAsync();
    }

    public async Task<T?> FindEntityByIdAsync<T>(int id) where T : class
    {
        return await DbContext.Set<T>().FindAsync(id);
    }

    public async Task<T?> FindEntityByCondition<T>(Expression<Func<T, bool>> condition) where T : class
    {
        return await DbContext.Set<T>().Where(condition).FirstOrDefaultAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await DbContext.Database.BeginTransactionAsync();
    }
}