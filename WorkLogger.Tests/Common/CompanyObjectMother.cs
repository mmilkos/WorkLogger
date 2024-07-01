using Microsoft.EntityFrameworkCore;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Interfaces;
using WorkLogger.Infrastructure.Persistence;

namespace WorkLogger.Tests.Common;

public static class CompanyObjectMother
{
    public static async Task<Company> CreateAsync(IWorkLoggerRepository repository, string name, List<User> employees)
    {
        var company = new Company
        {
            Name = name, 
        };

        await repository.AddCompanyAsync(company);
        return company;
    }
}