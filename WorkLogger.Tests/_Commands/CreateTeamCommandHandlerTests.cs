using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WorkLogger.Application._Commands.Teams;
using WorkLogger.Domain.DTOs;
using WorkLogger.Tests.Common;

namespace WorkLogger.Tests._Commands;

public class CreateTeamCommandHandlerTests : BaseTests
{
    [Fact]
    public async Task CreateTeamCommandHandler_ShouldCreateTeam()
    {
        // Arrange
        var dto = new CreateTeamDto()
        {
            Name = "TestName"
        };

        var command = new CreateTeamCommand(dto);

        // Act
        var result = await _mediator.Send(command);

        var teamFromDb = await _dbContext.Teams.FirstOrDefaultAsync();
        
        // Assert
        result.Success.Should().BeTrue();
        result.ErrorsList.Count.Should().Be(0);
        teamFromDb.Name.Should().Be(dto.Name);
    }
    
}