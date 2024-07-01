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
        try
        {
            await _mediator.Send(new RegisterUserCommand(registerDto));
            return Ok();

        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}