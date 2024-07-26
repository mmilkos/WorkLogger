using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.DTOs_responses;

namespace WorkLogger.Application._Queries.Teams;

public class CreateSummaryFileQuery : IRequest<OperationResult<ExcelFileResponseDto>>
{
    public CreateSummaryFileRequestDto Dto { get; set; }
    
    public CreateSummaryFileQuery(CreateSummaryFileRequestDto dto)
    {
        Dto = dto;
    }
}