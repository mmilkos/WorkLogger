using System.Security.Cryptography;
using System.Text;
using MediatR;
using WorkLogger.Domain;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Enums;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Commands.Users;

public class RegisterUserCommandHandler(IWorkLoggerRepository repository) : IRequestHandler<RegisterUserCommand, OperationResult<Unit>>
{
    private IWorkLoggerRepository _repository = repository;

    public async Task<OperationResult<Unit>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var operationResult = new OperationResult<Unit>();
        
        var dto = request.UserDto;
        Roles roles;
       

        var userAlreadyExist = await _repository.IsUserInDbAsync(dto.UserName);
        var company = await _repository.FindCompanyByIdAsync(dto.CompanyId);
        var isValidRole = Enum.IsDefined(typeof(Roles), dto.Roles);

        if (userAlreadyExist) operationResult.AddError(Errors.UserAlreadyExist);
        if (company == null) operationResult.AddError(Errors.CompanyDoesNotExist);
        if (isValidRole == false) operationResult.AddError(Errors.RoleDoesNotExist);

        

        if (operationResult.Success == false)
        {
            operationResult.StatusCode = (int)StatusCodesEnum.BadRequest;
            return operationResult;
        }
        
        roles = dto.Roles;
        
        User user;

        using ( var hmac = new HMACSHA512())
        {
            user = new User()
            {
                CompanyId = dto.CompanyId,
                Name = dto.Name,
                Surname = dto.Surname,
                UserName = dto.UserName,
                Role = roles,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.password)),
                PasswordSalt = hmac.Key
            };
        }

        await using (var transaction = await _repository.BeginTransactionAsync())
        {
            try
            {
                await _repository.AddUserAsync(user);
                await _repository.UpdateCompanyAsync(company);
               await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync(cancellationToken);
                operationResult.AddError(e.Message);
                operationResult.StatusCode = (int)StatusCodesEnum.InternalServerError;
            }
        }

        operationResult.StatusCode = (int)StatusCodesEnum.Ok;
        return operationResult;
    }
}