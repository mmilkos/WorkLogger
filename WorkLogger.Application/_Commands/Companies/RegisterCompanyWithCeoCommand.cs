using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;

namespace WorkLogger.Application._Commands.Companies;

public class RegisterCompanyWithCeoCommand : IRequest<OperationResult<CompanyIdAndNameResponseDto>>
{
    public RegisterCompanyRequestDto RequestDto;
    public RegisterCompanyWithCeoCommand(RegisterCompanyRequestDto requestDto)
    {
        RequestDto = requestDto;
    }
}