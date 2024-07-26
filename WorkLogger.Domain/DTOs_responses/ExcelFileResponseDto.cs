namespace WorkLogger.Domain.DTOs_responses;

public class ExcelFileResponseDto
{
    public string FileName { get; set; }
    public MemoryStream Stream { get; set; }
    public string MimeType => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
}