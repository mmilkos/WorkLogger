using MediatR;
using WorkLogger.Domain;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Enums;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Commands.Teams;

public class AssignUserToTeamCommandHandler : IRequestHandler<AssignUserToTeamCommand, OperationResult<Unit>>
{
    private IWorkLoggerRepository _repository;

    public AssignUserToTeamCommandHandler(IWorkLoggerRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<OperationResult<Unit>> Handle(AssignUserToTeamCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var result = new OperationResult<Unit>();
        
        var team = await _repository.FindEntityByIdAsync<Team>(dto.TeamId);
        var user = await _repository.FindEntityByIdAsync<User>(dto.UserId);

        result = Validate(result, team, user);
        
        if (result.Success == false) return result;

      
        team.TeamMembers.Add(user);
        user.SetTeam(team.Id);

        try
        {
            await _repository.UpdateEntityAsync(team);
            await _repository.UpdateEntityAsync(user);
        }
        catch (Exception e)
        {
            result.AddError(e.Message);
            result.ErrorType = ErrorTypesEnum.ServerError;
            return result;
        }

        return result;
    }
    
    private OperationResult<Unit> Validate(OperationResult<Unit> result, Team? team, User? user)
    {
        if (team == null) result.AddError(Errors.TeamDoesNotExist);
        if (user == null) result.AddError(Errors.UserDoesNotExist);
        if (result.Success == false) return result;
        
        var isManager = user.Role == Roles.Manager;
        if (isManager == false) return result;
        
        var teamHasManager = team.TeamMembers.Any(teamMember => teamMember.Role == Roles.Manager);
        if (isManager && teamHasManager) result.AddError(Errors.TeamAlreadyHasManager);

        return result;
    }
}