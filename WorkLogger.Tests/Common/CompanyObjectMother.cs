using Microsoft.EntityFrameworkCore;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Interfaces;
using WorkLogger.Infrastructure.Persistence;

namespace WorkLogger.Tests.Common;

public static class CompanyObjectMother
{
    public static async Task<Company> CreateAsync(WorkLoggerDbContext dbContext, string name)
    {
        var company = new Company(name: name);

        await dbContext.Companies.AddAsync(company);
        await dbContext.SaveChangesAsync();
        return company;
    }
}