using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;

namespace WorkLogger.Application._Commands.Teams;

public class UnAssignUserFromTeamCommand : IRequest<OperationResult<Unit>>
{
    public UnAssignUserFromTeamRequestDto Dto { get; set; }
    
    public UnAssignUserFromTeamCommand(UnAssignUserFromTeamRequestDto dto)
    {
        Dto = dto;
    }
}