using MediatR;
using WorkLogger.Domain;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Enums;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Commands.Teams;

public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand,OperationResult<Unit>>
{
    private IWorkLoggerRepository _repository;
    
    public CreateTeamCommandHandler(IWorkLoggerRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<OperationResult<Unit>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Unit>();
        
        var dto = request.Dto;

        var team = new Team()
        {
            Name = dto.Name,
            CompanyId = dto.CompanyId
        };

        try
        {
            await _repository.AddTeamAsync(team);
            return result;
        }
        catch (Exception e)
        {
          result.AddError(e.Message);
          result.ErrorType = ErrorTypesEnum.ServerError;
        }

        return result;
    }
}