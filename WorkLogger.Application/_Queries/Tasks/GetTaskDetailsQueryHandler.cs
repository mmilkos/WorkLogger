using System.Linq.Expressions;
using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.DTOs_responses;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Commands.Tasks;

public class GetTaskDetailsQueryHandler : IRequestHandler<GetTaskDetailsQuery, OperationResult<UserTaskDetailsResponseDto>>
{
    private IWorkLoggerRepository _repository;
    
    public GetTaskDetailsQueryHandler(IWorkLoggerRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<OperationResult<UserTaskDetailsResponseDto>> Handle(GetTaskDetailsQuery request, CancellationToken cancellationToken)
    {
        var taskId = request.TaskId;
        var result = new OperationResult<UserTaskDetailsResponseDto>();

        var task = await _repository.FindEntityByConditionAsync<UserTask>(condition: userTask => userTask.Id == taskId,
            include: new Expression<Func<UserTask, object>>[] 
            { 
                userTask => userTask.AssignedUser, 
                userTask => userTask.Author, 
                userTask => userTask.Team 
            });

        var assignedUserDto = new UserNameAndRoleResponseDto()
        {
            Id = task.AssignedUser.Id,
            Name = task.AssignedUser.Name,
            Surname = task.AssignedUser.Surname,
            Team = task.Team.Name,
            Role = task.AssignedUser.Role.ToString()
        };
        
        var authorDto = new UserNameAndRoleResponseDto()
        {
            Id = task.Author.Id,
            Name = task.Author.Name,
            Surname = task.Author.Surname,
            Team = task.Team.Name,
            Role = task.Author.Role.ToString()
        };

        var teamDto = new TeamResponseDto()
        {
            Id = task.TeamId,
            Name = task.Team.Name
        };
        
        result.Data = new UserTaskDetailsResponseDto()
        {
            Id = task.Id,
            AssignedUser = assignedUserDto,
            Author = authorDto,
            Team = teamDto,
            Name = task.Name,
            Description = task.Description,
            LoggedHours = task.LoggedHours,
            CreatedDate = task.CreatedDate,
            LastUpdateDate = task.LastUpdateDate
        };

        return result;
    }
}