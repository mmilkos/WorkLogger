using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;
using WorkLogger.Domain.Entities;

namespace WorkLogger.Domain.Interfaces;

public interface IWorkLoggerRepository
{
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task<bool> IsUserInDbAsync(string userName);
    Task<User?> FindUserByUsernameAsync(string userName);
    Task<List<T>> GetEntitiesPaged<T>(int companyId, int page, int pageSize) where T: BaseEntity;
    Task<int> GetEntitiesCount<T>(int companyId) where T: BaseEntity;
    Task AddAsync<T>(T entity) where T : class;
    Task<T?> FindEntityByIdAsync<T>(int id) where T : class;

}