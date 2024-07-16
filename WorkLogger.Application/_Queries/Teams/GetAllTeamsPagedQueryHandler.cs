using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.DTOs_responses;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Enums;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Queries.Teams;

public class GetAllTeamsPagedQueryHandler : IRequestHandler<GetAllTeamsPagedQuery,OperationResult<PagedResultResponseDto<TeamResponseDto>>>
{
    private IWorkLoggerRepository _repository;
    
    public GetAllTeamsPagedQueryHandler(IWorkLoggerRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<OperationResult<PagedResultResponseDto<TeamResponseDto>>> Handle(GetAllTeamsPagedQuery request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var result = new OperationResult<PagedResultResponseDto<TeamResponseDto>>();
      
        var count = await _repository.GetEntitiesCountAsync<Team>(team => team.CompanyId == dto.CompanyId);

        if (count == 0) return result;
        
        var teamsPaged = await _repository.GetEntitiesPagedAsync<Team>(
            condition: team => team.CompanyId == dto.CompanyId, 
                 pagingParams: dto);
        
        
        var teamResponseList = teamsPaged.Select(team => new TeamResponseDto
        {
            Id = team.Id,
            Name = team.Name
        }).ToList();

        result.Data = new PagedResultResponseDto<TeamResponseDto>()
        {
            DataList = teamResponseList,
            PageNumber = dto.Page,
            TotalRecords = count
        };

        return result;
    }
}