using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WorkLogger.Application._Commands.Teams;
using WorkLogger.Domain.DTOs;
using WorkLogger.Infrastructure.Repositories;
using WorkLogger.Tests.Common;

namespace WorkLogger.Tests._Commands;


public class UnAssignUserFromTeamCommandHandlerTests : BaseTests
{
    [Fact]
    public async Task ValidRequest_ShouldUnAssignUserFromTeam()
    {
        //Arrange
        var repository = new WorkLoggerRepository(_dbContext);
        
        var company = await CompanyObjectMother.CreateAsync(dbContext: _dbContext, "Test name");
        var team = await TeamObjectMother.CreateAsync(dbContext: _dbContext, companyId: company.Id, name: "testTeam");
        var user = await UserObjectMother.CreateAsync(
            dbContext: _dbContext,
            companyId: company.Id,
            teamId: team.Id);
        team = await TeamObjectMother.AddTeamMemberAsync(dbContext: _dbContext, team: team, teamMember: user);

        var dto = new UnAssignUserFromTeamRequestDto()
        {
            CompanyId = company.Id,
            UserId = user.Id,
            TeamId = team.Id
        };
        
        var handler = new UnAssignUserFromTeamCommandHandler(repository);
        var command = new UnAssignUserFromTeamCommand(dto);
        
        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        var userInDb = await _dbContext.Users.FindAsync(user.Id);
        var teamInDb = await _dbContext.Teams.Where(t => t.Id == team.Id)
            .Include(t => t.TeamMembers).FirstOrDefaultAsync();
        
        //Assert
        result.ErrorsList.Should().BeEmpty();
        result.Success.Should().BeTrue();
        teamInDb.TeamMembers.Should().NotContain(user);
        userInDb.TeamId.Should().BeNull();
    }
}