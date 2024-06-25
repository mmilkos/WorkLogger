using MediatR;
using WorkLogger.Domain.DTOs;

namespace WorkLogger.Application._Commands.Companies;

public class RegisterCompanyCommand : IRequest<Unit>
{
    public RegisterCompanyDto Dto;
    public RegisterCompanyCommand(RegisterCompanyDto dto)
    {
        Dto = dto;
    }
}