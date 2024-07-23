using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;

namespace WorkLogger.Application._Commands.Tasks;

public class UpdateTaskCommand : IRequest<OperationResult<Unit>>
{
    public UpdateTaskRequestDto Dto { get; }
    
    public UpdateTaskCommand(UpdateTaskRequestDto dto)
    {
        Dto = dto;
    }
    
}