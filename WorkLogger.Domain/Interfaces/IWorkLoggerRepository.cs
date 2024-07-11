using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;
using WorkLogger.Domain.DTOs;

namespace WorkLogger.Domain.Interfaces;

public interface IWorkLoggerRepository
{
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task<List<T>> GetEntitiesPagedAsync<T>(Expression<Func<T, bool>> condition ,PagedRequestDto pagingParams) where T: class;
    Task<int> GetEntitiesCountAsync<T>(Expression<Func<T, bool>> condition) where T: class;
    Task AddAsync<T>(T entity) where T : class;
    Task<T?> FindEntityByIdAsync<T>(int id) where T : class;
    Task<T?> FindEntityByConditionAsync<T>(Expression<Func<T, bool>> condition) where T : class;

}