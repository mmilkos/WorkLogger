using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;

namespace WorkLogger.Application._Queries.Users;

public class LoginUserQuery : IRequest<OperationResult<UserDto>>
{
    public LoginUserDto Dto { get; }
    
    public LoginUserQuery(LoginUserDto loginDto)
    {
        Dto = loginDto;
    }
    
}