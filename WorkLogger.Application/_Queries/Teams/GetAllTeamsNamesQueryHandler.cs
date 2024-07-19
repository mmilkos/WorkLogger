using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.DTOs_responses;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Queries.Teams;

public class GetAllTeamsNamesQueryHandler : IRequestHandler<GetAllTeamsNamesQuery, OperationResult<TeamsNamesResponseDto>>
{
    private IWorkLoggerRepository _repository;
    
    public GetAllTeamsNamesQueryHandler(IWorkLoggerRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<OperationResult<TeamsNamesResponseDto>> Handle(GetAllTeamsNamesQuery request, CancellationToken cancellationToken)
    {
        var companyId = request.CompanyId;
        var result = new OperationResult<TeamsNamesResponseDto>();

        var teams = await _repository.GetAllEntitiesAsync<Team>(condition: team => team.CompanyId == companyId);
        
        var names = teams.Select(team => new TeamResponseDto()
        {
            Id = team.Id,
            Name = team.Name
        }).ToList();
        
        result.Data = new TeamsNamesResponseDto()
        {
            Teams = names
        };;

        return result;
    }
}