using Microsoft.EntityFrameworkCore;
using WorkLogger.Domain.Entities;

namespace WorkLogger.Infrastructure.Persistence;

public class WorkLoggerDbContext(DbContextOptions<WorkLoggerDbContext> options) : DbContext(options)
{
    public DbSet<UserTask> UserTasks { get; set; }
    public DbSet<User> Users { get; set; }
};