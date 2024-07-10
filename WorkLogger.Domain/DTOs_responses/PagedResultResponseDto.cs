namespace WorkLogger.Domain.DTOs_responses;

public class PagedResultResponseDto<T>
{
    public List<T> Data { get; set; }
    public int PageNumber { get; set; }
    public int TotalRecords { get; set; }
}