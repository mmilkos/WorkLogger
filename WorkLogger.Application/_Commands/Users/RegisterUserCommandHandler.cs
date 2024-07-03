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
            operationResult.ErrorType = ErrorTypesEnum.BadRequest;
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
                UserName = dto.UserName.ToLower(),
                Role = roles,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
                PasswordSalt = hmac.Key
            };
        }
        
        try
        {
            await _repository.AddUserAsync(user); 
        }
        catch (Exception e)
        {
            operationResult.AddError(e.Message);
            operationResult.ErrorType = ErrorTypesEnum.ServerError;
        }
        
        return operationResult;
    }
}