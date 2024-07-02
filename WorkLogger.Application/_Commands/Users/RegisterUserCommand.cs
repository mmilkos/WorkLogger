using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;

namespace WorkLogger.Application._Commands;

public class RegisterUserCommand : RegisterUserDto, IRequest<OperationResult<Unit>>
{
    public RegisterUserDto UserDto;
    public RegisterUserCommand(RegisterUserDto registerUserDto)
    {
        UserDto = registerUserDto;
        
    }
    
}