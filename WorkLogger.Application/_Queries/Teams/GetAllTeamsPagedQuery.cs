using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.DTOs_responses;

namespace WorkLogger.Application._Queries.Teams;

public class GetAllTeamsPagedQuery : IRequest<OperationResult<PagedResultResponseDto<TeamResponseDto>>>
{
    public PagedRequestDto Dto { get; set; }
    public GetAllTeamsPagedQuery(PagedRequestDto dto)
    {
        Dto = dto;
    }
}

