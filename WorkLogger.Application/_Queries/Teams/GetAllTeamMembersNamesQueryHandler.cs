using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs_responses;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Queries.Teams;

public class GetAllTeamMembersNamesQueryHandler : IRequestHandler<GetAllTeamMembersNamesQuery, OperationResult<UsersNamesResponseDto>>
{
    private IWorkLoggerRepository _repository;
    
    public GetAllTeamMembersNamesQueryHandler(IWorkLoggerRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<OperationResult<UsersNamesResponseDto>> Handle(GetAllTeamMembersNamesQuery request, CancellationToken cancellationToken)
    {
        var teamId = request.TeamId;
        var companyId = request.CompanyId;
        var result = new OperationResult<UsersNamesResponseDto>();
        var names = new UsersNamesResponseDto()
        {
            Names = []
        };

        var users = await _repository.GetAllEntitiesAsync<User>(
            user => user.TeamId == teamId 
                    && user.CompanyId == companyId);

        foreach (var user in users) names.Names.Add(
                new UserNameAndRoleResponseDto()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname
                });

        result.Data = names;

        return result;
    }
}