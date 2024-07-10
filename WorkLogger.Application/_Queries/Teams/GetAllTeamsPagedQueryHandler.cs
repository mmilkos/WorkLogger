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
        
        List<Team> teamsPaged;
        int count;
        
        try
        {
             count = await _repository.GetEntitiesCount<Team>(dto.CompanyId);
             teamsPaged = await _repository.GetEntitiesPaged<Team>(
                 companyId: dto.CompanyId, 
                 page: dto.Page,
                 pageSize: dto.PageSize);
        }
        catch (Exception e)
        {
            result.AddError(e.Message);
            result.ErrorType = ErrorTypesEnum.ServerError;
            return result;
        }

        var teamResponseList = teamsPaged.Select(team => new TeamResponseDto { Name = team.Name }).ToList();

        result.Data = new PagedResultResponseDto<TeamResponseDto>()
        {
            Data = teamResponseList,
            PageNumber = dto.Page,
            TotalRecords = count
        };

        return result;
    }
}