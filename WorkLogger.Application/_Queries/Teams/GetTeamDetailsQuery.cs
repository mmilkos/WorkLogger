using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs_responses;

namespace WorkLogger.Application._Queries.Teams;

public class GetTeamDetailsQuery : IRequest<OperationResult<TeamDetailsResponseDto>>
{
    public int TeamId { get; }
    
    public GetTeamDetailsQuery(int teamId)
    {
        TeamId = teamId;
    }
}