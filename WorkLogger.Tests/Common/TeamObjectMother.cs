using WorkLogger.Domain.Entities;
using WorkLogger.Infrastructure.Persistence;

namespace WorkLogger.Tests.Common;

public class TeamObjectMother
{
    public static async Task<Team> CreateAsync(WorkLoggerDbContext dbContext, int companyId, string name, List<User>? teamMembers = null)
    {
        var team = new Team(companyId: companyId, name: name);
        team.TeamMembers = teamMembers ?? new List<User>() { };
        
        await dbContext.Teams.AddAsync(team);
        await dbContext.SaveChangesAsync();
        return team;
    }
}