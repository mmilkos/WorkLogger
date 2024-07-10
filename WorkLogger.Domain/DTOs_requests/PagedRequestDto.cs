namespace WorkLogger.Domain.DTOs;

public class PagedRequestDto
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int CompanyId { get; set; }
}