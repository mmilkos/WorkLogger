using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.DTOs_responses;

namespace WorkLogger.Application._Commands.Tasks;

public class GetAllCompanyTasksPagedQuery : IRequest<OperationResult<PagedResultResponseDto<UserTasksResponseDto>>>
{
    public PagedRequestDto Dto { get; }
    
    public GetAllCompanyTasksPagedQuery(PagedRequestDto dto)
    {
        Dto = dto;
    }
    
}