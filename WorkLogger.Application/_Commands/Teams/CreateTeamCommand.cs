using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;

namespace WorkLogger.Application._Commands.Teams;

public class CreateTeamCommand : IRequest<OperationResult<Unit>>
{
    public CreateTeamRequestDto RequestDto;
    
    public CreateTeamCommand(CreateTeamRequestDto requestDto)
    {
        RequestDto = requestDto;
    }
}