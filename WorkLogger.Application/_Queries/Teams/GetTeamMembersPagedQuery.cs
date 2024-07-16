using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.DTOs_responses;

namespace WorkLogger.Application._Queries.Teams;

public class GetTeamMembersPagedQuery : IRequest<OperationResult<PagedResultResponseDto<UserNameAndRoleResponseDto>>>
{
    public PagedRequestDto Dto { get; set; }
    public int TeamId { get; set; }
    
    public GetTeamMembersPagedQuery(PagedRequestDto dto, int teamId)
    {
        Dto = dto;
        TeamId = teamId;
    }
    
}