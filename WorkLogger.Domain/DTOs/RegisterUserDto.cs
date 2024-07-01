using WorkLogger.Domain.Enums;

namespace WorkLogger.Domain.DTOs;

public class RegisterUserDto
{
    public int CompanyId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string UserName { get; set; }
    public Role Role { get; set; }
   public string password { get; set; }
}