namespace WorkLogger.Domain.DTOs_responses;

public class UserTaskResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Team { get; set; }
    public UserNameAndRoleResponseDto AssignedUser { get; set; }
}