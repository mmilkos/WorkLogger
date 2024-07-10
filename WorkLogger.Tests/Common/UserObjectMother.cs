using System.Security.Cryptography;
using System.Text;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Enums;
using WorkLogger.Domain.Interfaces;
using WorkLogger.Infrastructure.Persistence;

namespace WorkLogger.Tests.Common;

public static class UserObjectMother
{
    public static async Task<User> CreateAsync(
        WorkLoggerDbContext dbContext, 
        int companyId,
        int? teamId = null,
        string name = "John", 
        string surname = "Doe", 
        string userName = "johndoe", 
        Roles roles = Roles.Employee,
        string password = "password")
    {
        User user;

        using (var hmac = new HMACSHA512())
        {
            user = new User.Builder()
                .WithCompanyInfo(companyId: companyId, teamId: null, role: roles)
                .WithUserCredentials(name: name, surname: surname, userName: userName.ToLower())
                .WithPassword(passwordHash: hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                    passwordSalt: hmac.Key)
                .Build();
        }

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        return user;
    }
}