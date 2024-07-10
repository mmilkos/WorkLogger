using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
    
    //Users
    public async Task<bool> IsUserInDbAsync(string userName)
    {
        return await DbContext.Users.AnyAsync(user => user.UserName == userName.ToLower());
    }

    public async Task<User?> FindUserByUsernameAsync(string userName)
    {
        return await DbContext.Users.FirstOrDefaultAsync(user => user.UserName == userName.ToLower());
    }
    
    //Generic
    public async Task<List<T>> GetEntitiesPaged<T>(int companyId, int page, int pageSize) where T : BaseEntity
    {
        var offset = (page - 1) * pageSize;
        var pagedResult = await DbContext.Set<T>().Where(entity => entity.CompanyId == companyId)
            .Skip(offset)
            .Take(pageSize)
            .ToListAsync();
      
        return pagedResult;
    }

    public async Task<int> GetEntitiesCount<T>(int companyId) where T : BaseEntity
    {
        var entities = DbContext.Set<T>().Where(entity => entity.CompanyId == companyId);

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

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await DbContext.Database.BeginTransactionAsync();
    }
}