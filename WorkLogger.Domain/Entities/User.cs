using System.ComponentModel.DataAnnotations.Schema;
using WorkLogger.Domain.Enums;

namespace WorkLogger.Domain.Entities;

[Table("Users")]
public class User
{
    public required int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string UserName { get; set; }
    public Role Role { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}