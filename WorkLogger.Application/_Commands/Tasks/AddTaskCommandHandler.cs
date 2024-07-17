using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Commands.Tasks;

public class AddTaskCommandHandler : IRequestHandler<AddTaskCommand, OperationResult<Unit>>
{
    private IWorkLoggerRepository _repository;
    
    public AddTaskCommandHandler(IWorkLoggerRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<OperationResult<Unit>> Handle(AddTaskCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        
        var result = new OperationResult<Unit>();
        
        var userTask = new UserTask()
        {
            AssignedUserId = dto.AssignedUserId,
            AuthorId = dto.AuthorId,
            Name = dto.Name,
            Description = dto.Description
        };

        try
        {
            await _repository.AddAsync(userTask);
        }
        catch (Exception e)
        {
           result.AddError(e.Message);
        }

        return result;
    }
}