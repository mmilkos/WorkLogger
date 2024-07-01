using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkLogger.Application._Commands;
using WorkLogger.Domain.DTOs;

namespace WorkLogger.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController(IMediator mediator) : ControllerBase
{
    private IMediator _mediator = mediator;
    
    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser(RegisterUserDto registerDto)
    {
       
        var result = await _mediator.Send(new RegisterUserCommand(registerDto));

        return StatusCode(result.StatusCode, result.ErrorsList);
    }
}