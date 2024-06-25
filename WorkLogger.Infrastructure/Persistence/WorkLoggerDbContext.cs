using Microsoft.EntityFrameworkCore;
using WorkLogger.Domain.Entities;

namespace WorkLogger.Infrastructure.Persistence;

public class WorkLoggerDbContext(DbContextOptions<WorkLoggerDbContext> options) : DbContext(options)
{
    public DbSet<UserTask> UserTasks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         // Companies
        modelBuilder.Entity<Company>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Company>()
            .Property(c => c.Name)
            .IsRequired();

        modelBuilder.Entity<Company>()
            .HasOne(c => c.CEO)
            .WithOne()
            .HasForeignKey<Company>(c => c.Id)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Company>()
            .HasMany(c => c.Employees)
            .WithOne(u => u.Company)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Users
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<User>()
            .Property(u => u.Name)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(u => u.Surname)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(u => u.UserName)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(u => u.PasswordHash)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(u => u.PasswordSalt)
            .IsRequired();

        modelBuilder.Entity<User>()
            .HasOne(u => u.Company)
            .WithMany(c => c.Employees)
            .IsRequired();

        // UserTasks
        modelBuilder.Entity<UserTask>()
            .HasKey(ut => ut.Id);

        modelBuilder.Entity<UserTask>()
            .Property(ut => ut.Name)
            .IsRequired();

        modelBuilder.Entity<UserTask>()
            .Property(ut => ut.Description)
            .IsRequired();

        modelBuilder.Entity<UserTask>()
            .Property(ut => ut.LoggedHours)
            .IsRequired();

        modelBuilder.Entity<UserTask>()
            .HasOne(ut => ut.AssignedUser)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserTask>()
            .HasOne(ut => ut.Author)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        base.OnModelCreating(modelBuilder);
    }
};