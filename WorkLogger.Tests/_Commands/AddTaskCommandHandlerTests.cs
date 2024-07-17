using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WorkLogger.Application._Commands.Tasks;
using WorkLogger.Domain.DTOs;
using WorkLogger.Infrastructure.Repositories;
using WorkLogger.Tests.Common;

namespace WorkLogger.Tests._Commands;

public class AddTaskCommandHandlerTests : BaseTests
{
    [Fact]
    public async Task ValidRequest_ShouldAddTask()
    {
        //Arrange
        var respository = new WorkLoggerRepository(_dbContext);
        var company = await CompanyObjectMother.CreateAsync(dbContext: _dbContext, name: "test");
        var assignedUser = await UserObjectMother.CreateAsync(dbContext: _dbContext, companyId: company.Id, name: "User");
        var author = await UserObjectMother.CreateAsync(dbContext: _dbContext, companyId: company.Id, name: "Author");

        var dto = new AddTaskRequestDto()
        {
            AssignedUserId = assignedUser.Id,
            AuthorId = author.Id,
            Name = "Task Name",
            Description = "Task Description"
        };

        var command = new AddTaskCommand(dto);

        var handler = new AddTaskCommandHandler(respository);
        
        //Act
        var result = await handler.Handle(command, CancellationToken.None);
        var taskInDb = await _dbContext.UserTasks.Include(task => task.AssignedUser)
            .Include(task => task.Author)
            .FirstOrDefaultAsync();
        
        //Assert
        result.ErrorsList.Should().BeEmpty();
        result.Success.Should().BeTrue();
        taskInDb.AssignedUserId.Should().Be(assignedUser.Id);
        taskInDb.AuthorId.Should().Be(author.Id);
        taskInDb.AssignedUser.Should().Be(assignedUser);
        taskInDb.Author.Should().Be(author);
        taskInDb.Name.Should().Be(dto.Name);
        taskInDb.Description.Should().Be(dto.Description);
        taskInDb.LoggedHours.Should().Be(0);
    }
}