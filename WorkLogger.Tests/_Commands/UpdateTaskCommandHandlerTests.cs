using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WorkLogger.Application._Commands.Tasks;
using WorkLogger.Domain;
using WorkLogger.Domain.DTOs;
using WorkLogger.Infrastructure.Repositories;
using WorkLogger.Tests.Common;

namespace WorkLogger.Tests._Commands;

public class UpdateTaskCommandHandlerTests : BaseTests
{
    [Fact]
    public async Task ValidRequest_ShouldUpdateTask()
    {
        //Arrange
        var repository = new WorkLoggerRepository(_dbContext);
        var company = await CompanyObjectMother.CreateAsync(dbContext: _dbContext, name: "testCompany");
        var team = await TeamObjectMother.CreateAsync(dbContext: _dbContext, companyId: company.Id, name: "testTeam");
        var user = await UserObjectMother.CreateAsync(dbContext: _dbContext, companyId: company.Id, teamId: team.Id);
        var task = await UserTaskObjectMother.CreateAsync(dbContext: _dbContext, assignedUserId: user.Id,
            authorId: user.Id, teamId: team.Id);

        var dto = new UpdateTaskRequestDto
        {
            Id = task.Id,
            Name = "New name",
            Description = "new desc",
            LoggedHours = 10
        };

        var command = new UpdateTaskCommand(dto);
        var handler = new UpdateTaskCommandHandler(repository);
        
        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        var updatedTask = await _dbContext.UserTasks.FirstOrDefaultAsync();
        
        //Assert
        result.ErrorsList.Should().BeEmpty();
        result.Success.Should().BeTrue();
        updatedTask.Should().NotBe(null);
        updatedTask.Id.Should().Be(dto.Id);
        updatedTask.Name.Should().Be(dto.Name);
        updatedTask.Description.Should().Be(dto.Description);
        updatedTask.LoggedHours.Should().Be(dto.LoggedHours);
    }

    [Fact]
    public async Task TaskDoesntExist_ShouldReturnError()
    {
        //Arrange
        var repository = new WorkLoggerRepository(_dbContext);
        
        var dto = new UpdateTaskRequestDto
        {
            Id = int.MaxValue,
            Name = "New name",
            Description = "new desc",
            LoggedHours = 10
        };
        
        var command = new UpdateTaskCommand(dto);
        var handler = new UpdateTaskCommandHandler(repository);
        
        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        var updatedTask = await _dbContext.UserTasks.FirstOrDefaultAsync();
        
        //Assert
        result.ErrorsList.Should().Contain(Errors.TaskDoesNotExist);
        result.Success.Should().BeFalse();
        updatedTask.Should().BeNull();
    }
}