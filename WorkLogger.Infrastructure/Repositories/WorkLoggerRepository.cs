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
    
    //Companies
    public async Task AddCompanyAsync(Company company)
    {
         await DbContext.Companies.AddAsync(company);
         await DbContext.SaveChangesAsync();
    }

    public async Task<Company?> FindCompanyByIdAsync(int id)
    {
        return await DbContext.Companies.FindAsync(id);
    }

    public async Task UpdateCompanyAsync(Company company)
    {
        DbContext.Companies.Update(company);
        await DbContext.SaveChangesAsync();
    }
    
    public async Task<List<Company>> GetAllCompaniesAsync()
    {
        var companies = await DbContext.Companies.ToListAsync();
        return companies;
    }
    
    //Users
    public async Task<bool> IsUserInDbAsync(string userName)
    {
        return await DbContext.Users.AnyAsync(user => user.UserName == userName.ToLower());
    }

    public async Task AddUserAsync(User user)
    {
        await DbContext.Users.AddAsync(user);
        await DbContext.SaveChangesAsync();
    }

    public async Task<User?> FindUserByUsernameAsync(string userName)
    {
        return await DbContext.Users.FirstOrDefaultAsync(user => user.UserName == userName.ToLower());
    }

    //Teams
    public async Task AddTeamAsync(Team team)
    {
        await DbContext.Teams.AddAsync(team);
        await DbContext.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await DbContext.Database.BeginTransactionAsync();
    }
}