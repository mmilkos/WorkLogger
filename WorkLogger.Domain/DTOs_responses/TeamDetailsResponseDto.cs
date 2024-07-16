namespace WorkLogger.Domain.DTOs_responses;

public class TeamDetailsResponseDto
{
    public int TeamId { get; set; }
    public string Name { get; set; }
    public UserNameAndRoleResponseDto Manager { get; set; }
}