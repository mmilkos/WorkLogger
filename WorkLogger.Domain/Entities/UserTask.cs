namespace WorkLogger.Domain.Entities;

public class UserTask
{
    public int Id { get; set; }
    public User AssignedUser { get; set; }
    public int AssignedUserId { get; set; }
    public User Author { get; set; }
    public int AuthorId { get; set; }
    public Team Team { get; set; }
    public int TeamId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float LoggedHours { get; set; } = 0;
    public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;
    public DateTime LastUpdateDate { get; set; } = DateTime.UtcNow;
}