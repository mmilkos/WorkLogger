using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WorkLogger.Application._Commands.Teams;
using WorkLogger.Domain.DTOs;
using WorkLogger.Infrastructure.Repositories;
using WorkLogger.Tests.Common;

namespace WorkLogger.Tests._Commands;

public class CreateTeamCommandHandlerTests : BaseTests
{
    [Fact]
    public async Task CreateTeamCommandHandler_ShouldCreateTeam()
    {
        var company = await CompanyObjectMother.CreateAsync(dbContext: _dbContext, name: "Test");
        // Arrange
        var dto = new CreateTeamRequestDto()
        {
            Name = "TestName",
            CompanyId = company.Id
        };

        var command = new CreateTeamCommand(dto);

        // Act
        var result = await _mediator.Send(command);

        var teamFromDb = await _dbContext.Teams.FirstOrDefaultAsync();
        
        // Assert
        result.ErrorsList.Should().Equal(new List<string>() { });
        result.Success.Should().BeTrue();
        teamFromDb.Name.Should().Be(dto.Name);
    }
    
}