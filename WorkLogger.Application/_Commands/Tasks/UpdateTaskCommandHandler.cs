using MediatR;
using WorkLogger.Domain;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Commands.Tasks;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, OperationResult<Unit>>
{
    private IWorkLoggerRepository _repository;
    
    public UpdateTaskCommandHandler(IWorkLoggerRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<OperationResult<Unit>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var result = new OperationResult<Unit>();

        var task = await _repository.FindEntityByIdAsync<UserTask>(dto.Id);

        if (task == null)
        {
            result.AddError(Errors.TaskDoesNotExist);
            return result;
        }

        task.Name = dto.Name;
        task.Description = dto.Description;
        task.LoggedHours = dto.LoggedHours;
        task.LastUpdateDate = DateTime.Now;
        
        try
        {
            await _repository.UpdateEntityAsync(task);
        }
        catch (Exception e)
        {
            result.AddError(e.Message);
            return result;
        }

        return result;
    }
}