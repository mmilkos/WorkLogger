using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Enums;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Commands.Companies;

public class RegisterCompanyCommandHandler : IRequestHandler<RegisterCompanyCommand, OperationResult<Unit>>
{
    private IWorkLoggerRepository _repository;
    public RegisterCompanyCommandHandler(IWorkLoggerRepository repository)
    {
        _repository = repository;
    }
    public async Task<OperationResult<Unit>> Handle(RegisterCompanyCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Unit>();
        
        var dto = request.Dto;

        var company = new Company();
        company.Name = dto.Name;
        
        try
        {
            await _repository.AddCompanyAsync(company);
        }
        catch (Exception e)
        {
            result.AddError(e.Message);
            result.ErrorType = ErrorTypesEnum.ServerError;
        }
        
        return result;
    }
}