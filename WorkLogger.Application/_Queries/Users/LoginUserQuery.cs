﻿using MediatR;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;

namespace WorkLogger.Application._Queries.Users;

public class LoginUserQuery : IRequest<OperationResult<UserResponseDto>>
{
    public LoginUserRequestDto RequestDto { get; }
    
    public LoginUserQuery(LoginUserRequestDto loginRequestDto)
    {
        RequestDto = loginRequestDto;
    }
    
}