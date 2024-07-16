using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs_responses;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Enums;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Queries.Users;

public class GetAllUsersPagedQueryHandler: IRequestHandler<GetAllUsersPagedQuery, OperationResult<PagedResultResponseDto<UserNameAndRoleResponseDto>>>
{
    private IWorkLoggerRepository _repository;
    public GetAllUsersPagedQueryHandler(IWorkLoggerRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<OperationResult<PagedResultResponseDto<UserNameAndRoleResponseDto>>> Handle(GetAllUsersPagedQuery request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var result = new OperationResult<PagedResultResponseDto<UserNameAndRoleResponseDto>>(); 
        
        
        var usersPaged = await _repository.GetEntitiesPagedAsync<User>(
                condition: user => user.CompanyId == dto.CompanyId,
                pagingParams: dto);

        var count = await _repository.GetEntitiesCountAsync<User>(condition: user => user.CompanyId == dto.CompanyId);

        var teams = await _repository.GetAllEntitiesAsync<Team>(team => team.CompanyId == dto.CompanyId);
        var teamsDict = teams.ToDictionary(team => team.Id, team => team.Name);
        
        var usersResponseList = usersPaged.Select(user => new UserNameAndRoleResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Team = user.TeamId.HasValue && teamsDict.TryGetValue(user.TeamId.Value, out var teamName) ? teamName : null,
            Surname = user.Surname,
            Role = user.Role.ToString(),
        }).ToList();

        result.Data = new PagedResultResponseDto<UserNameAndRoleResponseDto>()
        {
            DataList = usersResponseList,
            PageNumber = dto.Page,
            TotalRecords = count
        };

        return result;
    }
}