namespace WorkLogger.Domain.DTOs;

public class AssignUserToTeamRequestDto
{
    public int UserId { get; set; }
    public int TeamId { get; set; }
    public int? CompanyId { get; set; }
}