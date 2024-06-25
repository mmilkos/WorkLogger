using Microsoft.EntityFrameworkCore;
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


    public async Task<bool> IsUserInDbAsync(string userName)
    {
         return await DbContext.Users.AnyAsync(user => user.UserName == userName.ToLower());
    }

    public async Task AddCompanyAsync(Company company)
    {
         await DbContext.Companies.AddAsync(company);
         await DbContext.SaveChangesAsync();
    }
}