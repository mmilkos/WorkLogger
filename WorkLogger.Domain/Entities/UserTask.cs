namespace WorkLogger.Domain.Entities;

public class UserTask
{
    public required int Id { get; set; }
    public int AssignedUserId { get; set; }
    public int AuthorId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float LoggedHours { get; set; }
}