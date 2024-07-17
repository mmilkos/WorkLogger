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
    private readonly WorkLoggerDbContext _dbContext;
    
    public WorkLoggerRepository(WorkLoggerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<T>> GetEntitiesPagedAsync<T>(Expression<Func<T, bool>> condition,
        PagedRequestDto pagingParams, params Expression<Func<T, object>>[] include) where T : class
    {
        var query = _dbContext.Set<T>().Where(condition);
        foreach (var includeProperty in include) query = query.Include(includeProperty);
        
        var offset = (pagingParams.Page - 1) * pagingParams.PageSize;
        
        var pagedResult = await query
            .Skip(offset)
            .Take(pagingParams.PageSize)
            .ToListAsync();
      
        return pagedResult;
    }

    public async Task<int> GetEntitiesCountAsync<T>(Expression<Func<T, bool>> condition) where T : class
    {
        var entities = _dbContext.Set<T>().Where(condition);

        return await entities.CountAsync();
    }

    public async Task AddAsync<T>(T entity) where T: class 
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<T?> FindEntityByIdAsync<T>(int id) where T : class
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<T?> FindEntityByConditionAsync<T>(Expression<Func<T, bool>> condition,
        params Expression<Func<T, object>>[] include) where T : class
    {
        var query = _dbContext.Set<T>().Where(condition);

        foreach (var includeProperty in include) query = query.Include(includeProperty);
        
        return await query.FirstOrDefaultAsync();
    }

    public async Task<T> UpdateEntityAsync<T>(T entity) where T : class
    {
        _dbContext.Set<T>().Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<List<T>> GetAllEntitiesAsync<T>(Expression<Func<T, bool>> condition) where T : class
    { 
        return await _dbContext.Set<T>().Where(condition).ToListAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _dbContext.Database.BeginTransactionAsync();
    }
}