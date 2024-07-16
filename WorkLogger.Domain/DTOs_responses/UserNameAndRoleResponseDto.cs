namespace WorkLogger.Domain.DTOs_responses;

public class UserNameAndRoleResponseDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Team { get; set; }
    public string? Role { get; set; }
}