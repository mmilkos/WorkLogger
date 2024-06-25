using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.Entities;

namespace WorkLogger.Domain.Interfaces;

public interface IWorkLoggerRepository
{
    Task<bool> IsUserInDbAsync(string userName);
    Task AddCompanyAsync(Company company);
}