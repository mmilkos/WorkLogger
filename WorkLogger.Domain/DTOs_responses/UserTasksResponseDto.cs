namespace WorkLogger.Domain.DTOs_responses;

public class UserTasksResponseDto
{
    public string AssignedUserName { get; set; }
    public string AuthorName { get; set; }
    public string Name { get; set; }
    public float LoggedHours { get; set; }
}