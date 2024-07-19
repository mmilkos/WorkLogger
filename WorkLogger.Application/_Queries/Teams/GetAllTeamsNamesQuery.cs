using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs_responses;

namespace WorkLogger.Application._Queries.Teams;

public class GetAllTeamsNamesQuery : IRequest<OperationResult<TeamsNamesResponseDto>>
{
    public int? CompanyId { get; }
    
    public GetAllTeamsNamesQuery(int? companyId)
    {
        CompanyId = companyId;
    }
    
}