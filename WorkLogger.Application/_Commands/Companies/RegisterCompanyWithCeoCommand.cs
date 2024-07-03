using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;

namespace WorkLogger.Application._Commands.Companies;

public class RegisterCompanyWithCeoCommand : IRequest<OperationResult<CompanyIdAndNameDto>>
{
    public RegisterCompanyDto Dto;
    public RegisterCompanyWithCeoCommand(RegisterCompanyDto dto)
    {
        Dto = dto;
    }
}