using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkLogger.Application._Queries.Users;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.DTOs_responses;

namespace WorkLogger.Controllers;

[Authorize(Policy = Auth.TeamsManagementPolicy)]
[ApiController]
[Route("api/user")]
public class UserController(IMediator mediator) : ControllerBase 
{
    private IMediator _mediator = mediator;
    
    [HttpGet]
    public async Task<ActionResult<PagedResultResponseDto<UserNameAndRoleResponseDto>>> GetAllUsersPaged([FromQuery] int page, [FromQuery] int pageSize)
    {
        var companyId = GetCompanyId(User.Claims.ToList());
        if (companyId.HasValue == false) return BadRequest();
        
        var requestDto = new PagedRequestDto
        {
            Page = page, 
            PageSize = pageSize,
            CompanyId = companyId.Value
        };

        var result = await _mediator.Send(new GetAllUsersPagedQuery(requestDto));

        if (result.Success) return Ok(result.Data);

        return StatusCode(500, result.ErrorsList);
    }
    
    private int? GetCompanyId(List<Claim> claims)
    {
        var claim = claims.FirstOrDefault(claim => claim.Type == "CompanyId");
        return int.Parse(claim.Value);
    }
}