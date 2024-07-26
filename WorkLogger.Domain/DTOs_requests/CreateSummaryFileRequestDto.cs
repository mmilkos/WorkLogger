namespace WorkLogger.Domain.DTOs;

public class CreateSummaryFileRequestDto
{
    public int TeamId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}