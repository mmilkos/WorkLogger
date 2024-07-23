using WorkLogger.Domain.DTOs;

namespace WorkLogger.Domain.DTOs_responses;

public class UserTaskDetailsResponseDto
{
    public int Id { get; set; }
    public UserNameAndRoleResponseDto AssignedUser { get; set; }
    public UserNameAndRoleResponseDto Author { get; set; }
    public TeamResponseDto Team { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float LoggedHours { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdateDate { get; set; }
}