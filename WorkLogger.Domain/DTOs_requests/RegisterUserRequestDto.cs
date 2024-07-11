using WorkLogger.Domain.Enums;

namespace WorkLogger.Domain.DTOs;

public class RegisterUserRequestDto
{
    public int CompanyId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string UserName { get; set; }
    public int Role { get; set; }
   public string Password { get; set; }
}