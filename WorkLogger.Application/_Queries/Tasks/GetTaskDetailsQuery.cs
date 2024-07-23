using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs_responses;

namespace WorkLogger.Application._Commands.Tasks;

public class GetTaskDetailsQuery : IRequest<OperationResult<UserTaskDetailsResponseDto>>
{
    public int TaskId { get; private set; }
    
    public GetTaskDetailsQuery(int taskId)
    {
        TaskId = taskId;
    }
}