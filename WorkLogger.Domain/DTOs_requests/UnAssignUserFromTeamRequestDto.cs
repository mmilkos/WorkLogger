namespace WorkLogger.Domain.DTOs;

public class UnAssignUserFromTeamRequestDto
{
    public int UserId { get; set; }
    public int TeamId { get; set; }
    public int? CompanyId { get; set; }
}