using MediatR;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Commands.Companies;

public class RegisterCompanyCommandHandler : IRequestHandler<RegisterCompanyCommand>
{
    private IWorkLoggerRepository _repository;
    public RegisterCompanyCommandHandler(IWorkLoggerRepository repository)
    {
        _repository = repository;
    }
    public async Task<Unit> Handle(RegisterCompanyCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var company = new Company();
        company.Name = dto.Name;

        await _repository.AddCompanyAsync(company);
        
        return Unit.Value;
    }
}