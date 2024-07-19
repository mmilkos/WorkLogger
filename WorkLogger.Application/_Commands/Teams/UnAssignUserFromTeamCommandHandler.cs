using MediatR;
using WorkLogger.Domain;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Commands.Teams;

public class UnAssignUserFromTeamCommandHandler : IRequestHandler<UnAssignUserFromTeamCommand, OperationResult<Unit>>
{
    private IWorkLoggerRepository _repository;
    
    public UnAssignUserFromTeamCommandHandler(IWorkLoggerRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<OperationResult<Unit>> Handle(UnAssignUserFromTeamCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var result = new OperationResult<Unit>();
        var user = await _repository.FindEntityByIdAsync<User>(dto.UserId);
        var team = await _repository.FindEntityByConditionAsync<Team>(
            condition: team => team.Id == dto.TeamId,
            include: team => team.TeamMembers);
        
        result = Validate(user, team, result);
        
        if (!result.Success) return result;

        try
        {
            user.SetTeam(null);
            await _repository.UpdateEntityAsync(user);
            team.TeamMembers.Remove(user);

            await _repository.UpdateEntityAsync(team);
        }
        catch (Exception e)
        {
            result.AddError(e.Message);
        }
        
        return result;
    }

    private OperationResult<Unit> Validate(User? user, Team? team, OperationResult<Unit> result)
    {
        if (user == null) result.AddError(Errors.UserDoesNotExist);
        if (team == null) result.AddError(Errors.TeamDoesNotExist);

        if (user == null || team == null) return result;
        
        if (user.TeamId != team.Id) result.AddError(Errors.UserNotAssignedToTeam);

        return result;
    }
}