using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;

namespace WorkLogger.Application._Commands.Tasks;

public class AddTaskCommand : IRequest<OperationResult<Unit>>
{
    public AddTaskRequestDto Dto { get; }
    
    public AddTaskCommand(AddTaskRequestDto dto)
    {
        Dto = dto;
    }
}