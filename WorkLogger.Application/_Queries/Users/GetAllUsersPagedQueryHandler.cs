using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs_responses;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Enums;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Queries.Users;

public class GetAllUsersPagedQueryHandler: IRequestHandler<GetAllUsersPagedQuery, OperationResult<PagedResultResponseDto<UserListResponseDto>>>
{
    private IWorkLoggerRepository _repository;
    public GetAllUsersPagedQueryHandler(IWorkLoggerRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<OperationResult<PagedResultResponseDto<UserListResponseDto>>> Handle(GetAllUsersPagedQuery request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var result = new OperationResult<PagedResultResponseDto<UserListResponseDto>>();
        
        List<User> usersPaged;
        int count;

        try
        {
            usersPaged = await _repository.GetEntitiesPagedAsync<User>(
                condition: user => user.CompanyId == dto.CompanyId,
                pagingParams: dto);

            count = await _repository.GetEntitiesCountAsync<User>(condition: user => user.CompanyId == dto.CompanyId);
        }
        catch (Exception e)
        {
            result.ErrorsList.Add(e.Message);
            result.ErrorType = ErrorTypesEnum.ServerError;
            return result;
        }
        
        var usersResponseList = usersPaged.Select(user => new UserListResponseDto
        {
            Name = user.Name,
            Surname = user.Surname,
            Role = user.Role.ToString(),
        }).ToList();

        result.Data = new PagedResultResponseDto<UserListResponseDto>()
        {
            Data = usersResponseList,
            PageNumber = dto.Page,
            TotalRecords = count
        };

        return result;
    }
}