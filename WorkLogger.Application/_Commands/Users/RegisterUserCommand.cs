using MediatR;
using WorkLogger.Domain.DTOs;

namespace WorkLogger.Application._Commands;

public class RegisterUserCommand : RegisterUserDto, IRequest
{
    public RegisterUserDto UserDto;
    public RegisterUserCommand(RegisterUserDto registerUserDto)
    {
        UserDto = registerUserDto;
        
    }
    
}