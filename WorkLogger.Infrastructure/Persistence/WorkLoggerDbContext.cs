﻿using Microsoft.EntityFrameworkCore;
using WorkLogger.Domain.Entities;

namespace WorkLogger.Infrastructure.Persistence;

public class WorkLoggerDbContext(DbContextOptions<WorkLoggerDbContext> options) : DbContext(options)
{
    public DbSet<UserTask> UserTasks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Team> Teams { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Surname).HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasMaxLength(64);
            entity.Property(e => e.PasswordSalt).HasMaxLength(16);
            
            entity.HasOne<Team>(u => u.Team)
                .WithMany(p => p.TeamMembers)
                .HasForeignKey(e => e.TeamId);
        });

        modelBuilder.Entity<UserTask>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Description).HasColumnType("TEXT");
            entity.Property(e => e.LoggedHours).HasColumnType("FLOAT");
            entity.Property(e => e.CreatedDate).HasColumnType("DATE");
            entity.Property(e => e.LastUpdateDate).HasColumnType("DATE");
            
            entity.HasOne<User>(ut => ut.AssignedUser)
                .WithMany()
                .HasForeignKey(ut => ut.AssignedUserId);

            entity.HasOne<User>(ut => ut.Author)
                .WithMany()
                .HasForeignKey(ut => ut.AuthorId);

            entity.HasOne<Team>(ut => ut.Team)
                .WithMany()
                .HasForeignKey(ut => ut.TeamId);
        });
        
        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.CompanyId);
            entity.HasOne<Company>()
                .WithMany()
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
};