using System.Security.Cryptography;
using System.Text;
using MediatR;
using WorkLogger.Domain.Entities;
using WorkLogger.Domain.Enums;
using WorkLogger.Domain.Exceptions;
using WorkLogger.Domain.Interfaces;

namespace WorkLogger.Application._Commands.Users;

public class RegisterUserCommandHandler(IWorkLoggerRepository repository) : IRequestHandler<RegisterUserCommand>
{
    private IWorkLoggerRepository _repository = repository;

    public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var dto = request.UserDto;
        Role role;
       

        var userAlreadyExist = await _repository.IsUserInDbAsync(dto.UserName);
        var company = await _repository.FindCompanyByIdAsync(dto.CompanyId);
        var isValidRole = Enum.IsDefined(typeof(Role), dto.Role);

        if (userAlreadyExist) throw new UserAlreadyExistException();
        if (company == null) throw new CompanyDoesNotExistException();
        if (isValidRole) role = dto.Role;
        else throw new RoleDoesNotExistException();

        User user;

        using ( var hmac = new HMACSHA512())
        {
            user = new User()
            {
                CompanyId = dto.CompanyId,
                Name = dto.Name,
                Surname = dto.Surname,
                UserName = dto.UserName,
                Role = role,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.password)),
                PasswordSalt = hmac.Key
            };
        }

        await _repository.AddUserAsync(user);
        
        await _repository.UpdateCompanyAsync(company);
         
        return Unit.Value;
    }
}