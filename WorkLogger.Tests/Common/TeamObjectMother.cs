using WorkLogger.Domain.Entities;
using WorkLogger.Infrastructure.Persistence;

namespace WorkLogger.Tests.Common;

public class TeamObjectMother
{
    public static async Task<Team> CreateAsync(WorkLoggerDbContext dbContext, int companyId, string name)
    {
        var team = new Team(companyId: companyId, name: name);
        
        await dbContext.Teams.AddAsync(team);
        await dbContext.SaveChangesAsync();
        return team;
    }
}