using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Enums;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Commands.Companies;

public class RegisterCompanyWithCeoCommandHandler : IRequestHandler<RegisterCompanyWithCeoCommand, OperationResult<CompanyIdAndNameDto>>
{
    private IWorkLoggerRepository _repository;
    private IMediator _mediator;
    public RegisterCompanyWithCeoCommandHandler(IWorkLoggerRepository repository, IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }
    public async Task<OperationResult<CompanyIdAndNameDto>> Handle(RegisterCompanyWithCeoCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<CompanyIdAndNameDto>();
        
        var dto = request.Dto;

        var company = new Company();
        company.Name = dto.CompanyName;
        
        try
        {
            await _repository.AddCompanyAsync(company);
            result.Data = new CompanyIdAndNameDto()
            {
                Id = company.Id,
                Name = company.Name
            };
        }
        catch (Exception e)
        {
            result.AddError(e.Message);
            result.ErrorType = ErrorTypesEnum.ServerError;
            return result;
        }

        var ceoDto = new RegisterUserDto()
        {
            CompanyId = company.Id,
            Name = dto.Name,
            Surname = dto.Surname,
            UserName = dto.UserName,
            Roles = Roles.CEO,
            Password = dto.Password
        };

        var userResult = await _mediator.Send(new RegisterUserCommand(ceoDto), cancellationToken);

        foreach (var error in userResult.ErrorsList)
        {
            result.AddError(error);
        }
        
        
        return result;
    }
}