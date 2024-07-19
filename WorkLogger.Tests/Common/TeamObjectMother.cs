using WorkLogger.Domain.Entities;
using WorkLogger.Infrastructure.Persistence;

namespace WorkLogger.Tests.Common;

public class TeamObjectMother
{
    public static async Task<Team> CreateAsync(WorkLoggerDbContext dbContext, int companyId, string name)
    {
        var team = new Team(companyId: companyId, name: name);
        team.TeamMembers = new List<User>() { };
        
        await dbContext.Teams.AddAsync(team);
        await dbContext.SaveChangesAsync();
        return team;
    }

    public static async Task<Team> AddTeamMemberAsync(WorkLoggerDbContext dbContext, Team team, User teamMember)
    {
        team.TeamMembers.Add(teamMember);
        teamMember.SetTeam(team.Id);
        
         dbContext.Teams.Update(team);
         dbContext.Users.Update(teamMember);
         await dbContext.SaveChangesAsync();
         return team;
    }
}