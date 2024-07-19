using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs_responses;

namespace WorkLogger.Application._Queries.Teams;

public class GetAllTeamMembersNamesQuery : IRequest<OperationResult<UsersNamesResponseDto>>
{
    public int TeamId { get; }
    
    public GetAllTeamMembersNamesQuery(int teamId)
    {
        TeamId = teamId;
    }
}