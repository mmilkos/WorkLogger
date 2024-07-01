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
        IWorkLoggerRepository repository, 
        int companyId,
        string name = "John", 
        string surname = "Doe", 
        string userName = "johndoe", 
        Role role = Role.Employee,
        string password = "password")
    {
        User user;

        using (var hmac = new HMACSHA512())
        {
            user = new User()
            {
                CompanyId = companyId,
                Name = name,
                Surname = surname,
                UserName = userName,
                Role = role,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key
            };
        }

        await repository.AddUserAsync(user);
        return user;
    }
}