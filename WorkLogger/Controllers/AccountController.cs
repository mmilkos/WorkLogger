using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkLogger.Application._Commands;
using WorkLogger.Application._Queries.Users;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.Enums;

namespace WorkLogger.Controllers;

[ApiController]
[Route("api/account")]
[AllowAnonymous]
public class AccountController(IMediator mediator) : ControllerBase
{
    private IMediator _mediator = mediator;
    
    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser(RegisterUserDto registerDto)
    {
        var result = await _mediator.Send(new RegisterUserCommand(registerDto));

        if (result.Success) return Created();
        
        switch (result.ErrorType)
        {
            case ErrorTypesEnum.BadRequest: return BadRequest(result.ErrorsList);
            default: return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorsList);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> LoginUser(LoginUserDto loginDto)
    {
        var result = await _mediator.Send(new LoginUserQuery(loginDto));

        if (result.Success) return Ok(result.Data);

        switch (result.ErrorType)
        {
            case ErrorTypesEnum.Unauthorized: return Unauthorized(result.ErrorsList);
            default: return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorsList);
        }
    }
}