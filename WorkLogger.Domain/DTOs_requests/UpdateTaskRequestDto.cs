namespace WorkLogger.Domain.DTOs;

public class UpdateTaskRequestDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float LoggedHours { get; set; }
}