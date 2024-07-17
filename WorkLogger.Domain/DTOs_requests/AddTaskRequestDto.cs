namespace WorkLogger.Domain.DTOs;

public class AddTaskRequestDto
{
    public int AssignedUserId { get; set; }
    public int AuthorId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}