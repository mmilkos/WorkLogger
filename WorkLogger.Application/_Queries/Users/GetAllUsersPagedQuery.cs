using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.DTOs_responses;

namespace WorkLogger.Application._Queries.Users;

public class GetAllUsersPagedQuery : IRequest<OperationResult<PagedResultResponseDto<UserNameAndRoleResponseDto>>>
{
    public PagedRequestDto Dto { get; set; }
    
    public GetAllUsersPagedQuery(PagedRequestDto dto)
    {
        Dto = dto;
    }
}