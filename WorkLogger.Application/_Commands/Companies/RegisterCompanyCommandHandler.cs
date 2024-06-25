using MediatR;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Commands.Companies;

public class RegisterCompanyCommandHandler(IWorkLoggerRepository repository) : IRequestHandler<RegisterCompanyCommand>
{
    public async Task<Unit> Handle(RegisterCompanyCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var company = new Company();
        company.Name = dto.Name;

        await repository.AddCompanyAsync(company);
        
        return Unit.Value;
    }
}