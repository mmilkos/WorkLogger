using System.Linq.Expressions;
using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs_responses;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Commands.Tasks;

public class GetAllCompanyTasksPagedQueryHandler : IRequestHandler<GetAllCompanyTasksPagedQuery, OperationResult<PagedResultResponseDto<UserTaskResponseDto>>>
{
    private IWorkLoggerRepository _repository;
    
    public GetAllCompanyTasksPagedQueryHandler(IWorkLoggerRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<PagedResultResponseDto<UserTaskResponseDto>>> Handle(GetAllCompanyTasksPagedQuery request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        
        var result = new OperationResult<PagedResultResponseDto<UserTaskResponseDto>>()
        {
            Data = new PagedResultResponseDto<UserTaskResponseDto>(){}
        };

        var taskPaged = await _repository.GetEntitiesPagedAsync<UserTask>(
            condition: userTask => userTask.AssignedUser.CompanyId == dto.CompanyId,
            pagingParams: dto,
            include: new Expression<Func<UserTask, object>>[] 
            { 
                userTask => userTask.AssignedUser, 
                userTask => userTask.Author, 
                userTask => userTask.Team 
            });
        var totalRecords =
            await _repository.GetEntitiesCountAsync<UserTask>(condition: userTask =>
                userTask.AssignedUser.CompanyId == dto.CompanyId);

        foreach (var taskResponse in taskPaged.Select(task => new UserTaskResponseDto()
                 {
                     Id = task.Id,
                     Name = task.Name,
                     Team = task.Team.Name,
                     AssignedUser = new UserNameAndRoleResponseDto()
                     {
                         Id = task.AssignedUser.Id,
                         Name = task.AssignedUser.Name,
                         Surname = task.AssignedUser.Surname,
                         Team = task.Team.Name,
                         Role = task.AssignedUser.Role.ToString(),
                     }
                     
                 })) result.Data.DataList.Add(taskResponse);

        result.Data.PageNumber = dto.Page;
        result.Data.TotalRecords = totalRecords;

        return result;
    }
}