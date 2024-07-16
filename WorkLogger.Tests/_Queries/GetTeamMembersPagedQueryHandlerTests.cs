using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WorkLogger.Application._Queries.Teams;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.Entities;
using WorkLogger.Infrastructure.Repositories;
using WorkLogger.Tests.Common;

namespace WorkLogger.Tests._Queries;

public class GetTeamMembersPagedQueryHandlerTests : BaseTests
{
    [Fact]
    public async Task ValidRequest_ShouldReturnTeamMembersPaged()
    {
        //Arrange
        var repository = new WorkLoggerRepository(_dbContext);
        var company = await CompanyObjectMother.CreateAsync(dbContext: _dbContext, name: "Test Company" );
        var team = await TeamObjectMother.CreateAsync(dbContext: _dbContext, companyId: company.Id, name: "team1");
        var team2 = await TeamObjectMother.CreateAsync(dbContext: _dbContext, companyId: company.Id, name: "team2");
        

        var teamMember = await UserObjectMother.CreateAsync(dbContext: _dbContext,
            companyId: company.Id,
            teamId: team.Id);
        
        var nonTeamMember = await UserObjectMother.CreateAsync(dbContext: _dbContext,
            companyId: company.Id,
            teamId: team2.Id);

        team = await TeamObjectMother.AddTeamMemberAsync(dbContext: _dbContext, team: team, teamMember: teamMember);
        
        team2 = await TeamObjectMother.AddTeamMemberAsync(dbContext: _dbContext, team: team2, teamMember: nonTeamMember);
        

        var dto = new PagedRequestDto()
        {
            Page = 1,
            PageSize = 5,
            CompanyId = company.Id
        };
        
        //Act
        var queryHandler = new GetTeamMembersPagedQueryHandler(repository);
        var query = new GetTeamMembersPagedQuery(dto, teamId: team.Id);
        
        var result = await queryHandler.Handle(query, CancellationToken.None);
        
        
        //Assert
        result.ErrorsList.Should().BeEmpty();
        result.Success.Should().BeTrue();
        result.Data.DataList.Should().NotBeNullOrEmpty();
        result.Data.DataList.Count.Should().Be(team.TeamMembers.Count);
        result.Data.DataList[0].Id.Should().Be(teamMember.Id);
        result.Data.DataList[0].Name.Should().Be(teamMember.Name);
        result.Data.DataList[0].Surname.Should().Be(teamMember.Surname);
        result.Data.DataList[0].Team.Should().Be(team.Name);
        result.Data.DataList[0].Role.Should().Be(teamMember.Role.ToString());
    }
}