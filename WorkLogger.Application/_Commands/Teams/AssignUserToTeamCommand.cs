using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;

namespace WorkLogger.Application._Commands.Teams;

public class AssignUserToTeamCommand : IRequest<OperationResult<Unit>>
{
    public AssignUserToTeamRequestDto Dto;
    
    public AssignUserToTeamCommand(AssignUserToTeamRequestDto dto)
    {
        Dto = dto;
    }
    
}