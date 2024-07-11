using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WorkLogger.Application._Commands.Teams;
using WorkLogger.Domain;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Enums;
using WorkLogger.Tests.Common;
using WorkLogger.Infrastructure.Repositories;

namespace WorkLogger.Tests._Commands;

public class AssignUserToTeamCommandHandlerTests : BaseTests    
{
    [Fact]
    public async Task ValidRequest_ShouldAssignUserToTeam()
    {
        //Arrange
        var repository = new WorkLoggerRepository(_dbContext);
        
        var company = await CompanyObjectMother.CreateAsync(dbContext: _dbContext, name: "testCompany");
        var user = await UserObjectMother.CreateAsync(dbContext: _dbContext, companyId: company.Id);
        var team = await TeamObjectMother.CreateAsync(dbContext: _dbContext, name: "testTeam", companyId: company.Id);

        var dto = new AssignUserToTeamRequestDto()
        {
            CompanyId = company.Id,
            TeamId = team.Id,
            UserId = user.Id
        };

        var command = new AssignUserToTeamCommand(dto);

        //Act
        var handler = new AssignUserToTeamCommandHandler(repository);
        
        var result = await handler.Handle(command, CancellationToken.None);
        
        
        var userIndDb = await _dbContext.Users.FindAsync(user.Id);
        var teamInDb = _dbContext.Teams
            .Include(t => t.TeamMembers)
            .ToList().FirstOrDefault();

        //Assert
        result.ErrorsList.Should().BeEmpty();
        result.Success.Should().BeTrue();
        userIndDb.Should().NotBe(null);
        teamInDb.Should().NotBe(null);
        userIndDb.TeamId.Should().Be(teamInDb.Id);
        teamInDb.TeamMembers.Should().Contain(userIndDb);
    }

    [Fact]
    public async Task UserDoesntExist_ReturnError()
    {
        //Arrange
        var repository = new WorkLoggerRepository(_dbContext);
        
        var company = await CompanyObjectMother.CreateAsync(dbContext: _dbContext, name: "testCompany");
        var team = await TeamObjectMother.CreateAsync(dbContext: _dbContext, name: "testTeam", companyId: company.Id);

        var dto = new AssignUserToTeamRequestDto()
        {
            CompanyId = company.Id,
            TeamId = team.Id,
            UserId = int.MaxValue
        };

        var command = new AssignUserToTeamCommand(dto);

        //Act
        var handler = new AssignUserToTeamCommandHandler(repository);
        
        var result = await handler.Handle(command, CancellationToken.None);
        var teamInDb = _dbContext.Teams
            .Include(t => t.TeamMembers)
            .ToList().FirstOrDefault();

        
        //Assert
        result.ErrorsList.Should().Contain(Errors.UserDoesNotExist);
        result.Success.Should().BeFalse();
        teamInDb.TeamMembers.Should().BeEmpty();
    }
    
    [Fact]
    public async Task TeamDoesntExist_ReturnError()
    {
        //Arrange
        var repository = new WorkLoggerRepository(_dbContext);
        
        var company = await CompanyObjectMother.CreateAsync(dbContext: _dbContext, name: "testCompany");
        var user = await UserObjectMother.CreateAsync(dbContext: _dbContext, companyId: company.Id);
        var team = await TeamObjectMother.CreateAsync(dbContext: _dbContext, name: "testTeam", companyId: company.Id);

        var dto = new AssignUserToTeamRequestDto()
        {
            CompanyId = company.Id,
            TeamId = int.MaxValue,
            UserId = user.Id
        };

        var command = new AssignUserToTeamCommand(dto);

        //Act
        var handler = new AssignUserToTeamCommandHandler(repository);
        
        var result = await handler.Handle(command, CancellationToken.None);
        var teamInDb = _dbContext.Teams
            .Include(t => t.TeamMembers)
            .ToList().FirstOrDefault();

        
        //Assert
        result.ErrorsList.Should().Contain(Errors.TeamDoesNotExist);
        result.Success.Should().BeFalse();
        teamInDb.TeamMembers.Should().BeEmpty();
    }
    
    [Fact]
    public async Task TeamAlreadyHasManager_ReturnError()
    {
        //Arrange
        var repository = new WorkLoggerRepository(_dbContext);
        
        var company = await CompanyObjectMother.CreateAsync(dbContext: _dbContext, name: "testCompany");
        var manager = await UserObjectMother.CreateAsync(dbContext: _dbContext, companyId: company.Id, roles: Roles.Manager);
        var secondManager = await UserObjectMother.CreateAsync(dbContext: _dbContext, companyId: company.Id, roles: Roles.Manager);
        
        var team = await TeamObjectMother.CreateAsync(dbContext: _dbContext, 
            name: "testTeam", 
            companyId: company.Id, 
            teamMembers: new List<User>() { manager });

        var dto = new AssignUserToTeamRequestDto()
        {
            CompanyId = company.Id,
            TeamId = team.Id,
            UserId = secondManager.Id
        };

        var command = new AssignUserToTeamCommand(dto);

        //Act
        var handler = new AssignUserToTeamCommandHandler(repository);
        
        var result = await handler.Handle(command, CancellationToken.None);
        
        
        var teamInDb = _dbContext.Teams
            .Include(t => t.TeamMembers)
            .ToList().FirstOrDefault();

        //Assert
        result.ErrorsList.Should().Contain(Errors.TeamAlreadyHasManager);
        result.Success.Should().BeFalse();
        teamInDb.Should().NotBe(null);
        teamInDb.TeamMembers.Should().NotContain(secondManager);
    }
}