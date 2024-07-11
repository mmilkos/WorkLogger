using System.Security.Claims;
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
    public async Task<ActionResult> RegisterUser(RegisterUserRequestDto registerRequestDto)
    {
        
        var companyId = GetCompanyId(User.Claims.ToList());
        if (companyId.HasValue == false) return BadRequest();
        registerRequestDto.CompanyId = companyId.Value; 
        
        var result = await _mediator.Send(new RegisterUserRequestCommand(registerRequestDto));

        if (result.Success) return Created();
        
        switch (result.ErrorType)
        {
            case ErrorTypesEnum.BadRequest: return BadRequest(result.ErrorsList);
            default: return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorsList);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserLoginResponseDto>> LoginUser(LoginUserRequestDto loginRequestDto)
    {
        var result = await _mediator.Send(new LoginUserQuery(loginRequestDto));

        if (result.Success) return Ok(result.Data);

        switch (result.ErrorType)
        {
            case ErrorTypesEnum.Unauthorized: return Unauthorized(result.ErrorsList);
            default: return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorsList);
        }
    }
    
    private int? GetCompanyId(List<Claim> claims)
    {
        var claim = claims.Where(claim => claim.Type == "CompanyId").FirstOrDefault();
        return int.Parse(claim.Value);
    }
}