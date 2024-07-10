using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;

namespace WorkLogger.Application._Commands;

public class RegisterUserRequestCommand : RegisterUserRequestDto, IRequest<OperationResult<Unit>>
{
    public RegisterUserRequestDto UserRequestDto;
    public RegisterUserRequestCommand(RegisterUserRequestDto registerUserRequestDto)
    {
        UserRequestDto = registerUserRequestDto;
        
    }
    
}