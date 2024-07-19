using System.Linq.Expressions;
using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs_responses;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Commands.Tasks;

public class GetAllCompanyTasksPagedQueryHandler : IRequestHandler<GetAllCompanyTasksPagedQuery, OperationResult<PagedResultResponseDto<UserTasksResponseDto>>>
{
    private IWorkLoggerRepository _repository;
    
    public GetAllCompanyTasksPagedQueryHandler(IWorkLoggerRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<PagedResultResponseDto<UserTasksResponseDto>>> Handle(GetAllCompanyTasksPagedQuery request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        
        var result = new OperationResult<PagedResultResponseDto<UserTasksResponseDto>>()
        {
            Data = new PagedResultResponseDto<UserTasksResponseDto>(){}
        };

        var taskPaged = await _repository.GetEntitiesPagedAsync<UserTask>(
            condition: userTask => userTask.AssignedUser.CompanyId == dto.CompanyId,
            pagingParams: dto,
            include: new Expression<Func<UserTask, object>>[] 
            { 
                userTask => userTask.AssignedUser, 
                userTask => userTask.Author 
            });

        foreach (var taskResponse in taskPaged.Select(task => new UserTasksResponseDto()
                 {
                     Name = task.Name,
                     AssignedUserName = task.AssignedUser.Name,
                     AuthorName = task.Author.Name,
                     LoggedHours = task.LoggedHours
                 })) result.Data.DataList.Add(taskResponse);
        

        return result;
    }
}