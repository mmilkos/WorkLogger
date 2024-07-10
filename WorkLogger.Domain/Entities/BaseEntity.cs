namespace WorkLogger.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
}