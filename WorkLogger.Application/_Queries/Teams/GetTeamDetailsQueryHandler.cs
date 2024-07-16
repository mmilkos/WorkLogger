using MediatR;
using WorkLogger.Domain;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs_responses;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Enums;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Queries.Teams;

public class GetTeamDetailsQueryHandler : IRequestHandler<GetTeamDetailsQuery, OperationResult<TeamDetailsResponseDto>>
{
    private IWorkLoggerRepository _repository;
    
    public GetTeamDetailsQueryHandler(IWorkLoggerRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<OperationResult<TeamDetailsResponseDto>> Handle(GetTeamDetailsQuery request, CancellationToken cancellationToken)
    {
        var teamId = request.TeamId;
        var result = new OperationResult<TeamDetailsResponseDto>();

        var team = await _repository.FindEntityByConditionAsync<Team>(team => team.Id == teamId,
            include: team => team.TeamMembers);

        if (team == null)
        {
            result.AddError(Errors.TeamDoesNotExist);
            return result;
        }
        
        var manager = team.TeamMembers.Where(teamMemer => teamMemer.Role == Roles.Manager).FirstOrDefault();

        var response = new TeamDetailsResponseDto()
        {
            Name = team.Name,
            TeamId = team.Id,
        };

        if (manager != null)
        {
            response.Manager = new UserNameAndRoleResponseDto()
            {
                Name = manager.Name,
                Surname = manager.Surname,
                Role = manager.Role.ToString()
            };
        }
        
        result.Data = response;
        return result;
    }
}