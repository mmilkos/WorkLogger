using MediatR;
using WorkLogger.Domain;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs_responses;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Queries.Teams;

public class GetTeamMembersPagedQueryHandler : IRequestHandler<GetTeamMembersPagedQuery, OperationResult<PagedResultResponseDto<UserNameAndRoleResponseDto>> >
{
    private readonly IWorkLoggerRepository _repository;
    public GetTeamMembersPagedQueryHandler(IWorkLoggerRepository repository)
    {
        _repository = repository;
    }
    public async Task<OperationResult<PagedResultResponseDto<UserNameAndRoleResponseDto>>> Handle(GetTeamMembersPagedQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<PagedResultResponseDto<UserNameAndRoleResponseDto>>
        {
            Data = new PagedResultResponseDto<UserNameAndRoleResponseDto>()
        };

        var dto = request.Dto;
        var teamId = request.TeamId;

        var count = await _repository.GetEntitiesCountAsync<User>(condition: user => user.TeamId == teamId);

        if (count == 0) return result;

        var teamMembers =  await _repository.GetEntitiesPagedAsync<User>(
            condition: user => user.TeamId == teamId && user.CompanyId == dto.CompanyId, 
            pagingParams: dto,
            include: user => user.Team);
        
        foreach (var user in teamMembers)
        {
            var teamName = user.TeamId.HasValue ? user.Team.Name : null;
            
            var userDto = new UserNameAndRoleResponseDto()
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Team = teamName,
                Role = user.Role.ToString()
            };
            result.Data.DataList.Add(userDto);
        }

        result.Data.PageNumber = dto.Page;
        result.Data.TotalRecords = count;

        return result;
    }
}