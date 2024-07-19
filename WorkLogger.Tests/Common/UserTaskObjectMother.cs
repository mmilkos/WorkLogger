using WorkLogger.Domain.Entities;
using WorkLogger.Infrastructure.Persistence;

namespace WorkLogger.Tests.Common;

public class UserTaskObjectMother
{
    public static async Task<UserTask> CreateAsync(
        WorkLoggerDbContext dbContext,
        int assignedUserId,
        int authorId,
        int teamId,
        float loggedHours = 0,
        string name = "Test task", 
        string description = "Test desc")
    {
        var userTask = new UserTask()
        {
            AssignedUserId = assignedUserId,
            AuthorId = authorId,
            TeamId = teamId,
            LoggedHours = loggedHours,
            Name = name,
            Description = description
        };

        await dbContext.UserTasks.AddAsync(userTask);
        await dbContext.SaveChangesAsync();
        return userTask;
    }
}