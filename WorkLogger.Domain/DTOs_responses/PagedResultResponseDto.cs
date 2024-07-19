namespace WorkLogger.Domain.DTOs_responses;

public class PagedResultResponseDto<T>
{
    public List<T> DataList { get; set; } = new List<T>(){};
    public int PageNumber { get; set; }
    public int TotalRecords { get; set; }
}