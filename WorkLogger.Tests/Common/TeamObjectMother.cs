using WorkLogger.Domain.Entities;
using WorkLogger.Infrastructure.Persistence;

namespace WorkLogger.Tests.Common;

public class TeamObjectMother
{
    public static async Task<Team> Create(WorkLoggerDbContext dbContext, int companyId, string name)
    {
        var team = new Team(companyId: companyId, name: name);
        
        dbContext.Teams.Add(team);
        await dbContext.SaveChangesAsync();
        return team;
    }
}