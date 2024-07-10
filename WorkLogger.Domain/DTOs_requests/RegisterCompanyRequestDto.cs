using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Enums;

namespace WorkLogger.Domain.DTOs;

public class RegisterCompanyRequestDto
{
    public string CompanyName { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}