using System.Security.Cryptography;
using System.Text;
using MediatR;
using WorkLogger.Domain;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Enums;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Commands.Users;

public class RegisterUserCommandHandler(IWorkLoggerRepository repository) : IRequestHandler<RegisterUserRequestCommand, OperationResult<Unit>>
{
    private IWorkLoggerRepository _repository = repository;

    public async Task<OperationResult<Unit>> Handle(RegisterUserRequestCommand request, CancellationToken cancellationToken)
    {
        var operationResult = new OperationResult<Unit>();
        
        var dto = request.UserRequestDto;
        Roles roles;
       

        var userInDb = await _repository.FindEntityByConditionAsync<User>(user => user.UserName == dto.UserName);
        var company = await _repository.FindEntityByIdAsync<Company>(dto.CompanyId);
        var isValidRole = Enum.IsDefined(typeof(Roles), dto.Role);
        var isCorrectPasswordLen = dto.Password.Length >= 8;

        if (userInDb != null) operationResult.AddError(Errors.UserAlreadyExist);
        if (company == null) operationResult.AddError(Errors.CompanyDoesNotExist);
        if (isValidRole == false) operationResult.AddError(Errors.RoleDoesNotExist);
        if (isCorrectPasswordLen == false) operationResult.AddError(Errors.ShortPassword);
        
        
        if (operationResult.Success == false)
        {
            operationResult.ErrorType = ErrorTypesEnum.BadRequest;
            return operationResult;
        }
        
        User user;

        using ( var hmac = new HMACSHA512())
        {
            user = new User.Builder()
                .WithCompanyInfo(companyId: dto.CompanyId, teamId: null, role: (Roles)dto.Role)
                .WithUserCredentials(name: dto.Name, surname: dto.Surname, userName: dto.UserName)
                .WithPassword(passwordHash: hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
                    passwordSalt: hmac.Key)
                .Build();
        }
        
        try
        {
            await _repository.AddAsync(user); 
        }
        catch (Exception e)
        {
            operationResult.AddError(e.Message);
            operationResult.ErrorType = ErrorTypesEnum.ServerError;
        }
        
        return operationResult;
    }
}