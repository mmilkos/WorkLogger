using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkLogger.Application._Commands.Teams;
using WorkLogger.Application._Queries.Teams;
using WorkLogger.Domain.Common;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.DTOs_responses;
using WorkLogger.Domain.Entities;

namespace WorkLogger.Controllers;

[ApiController]
[Route("api/team")]
public class TeamController(IMediator mediator) : ControllerBase
{
    private IMediator _mediator = mediator;

    [HttpPost("create")]
    public async Task<ActionResult> CreateTeam(CreateTeamRequestDto requestDto)
    {
        var companyId = GetCompanyId(User.Claims.ToList());
        if (companyId.HasValue == false) return BadRequest();

        requestDto.CompanyId = companyId.Value; 
        
        var result = await _mediator.Send(new CreateTeamCommand(requestDto));

        if (result.Success) return Created();
        
        return StatusCode(500, result.ErrorsList);
    }
    
    [HttpGet]
    public async Task<ActionResult<PagedResultResponseDto<TeamResponseDto>>> GetAllTeamsPaged([FromQuery] int page, [FromQuery] int pageSize)
    {
        var companyId = GetCompanyId(User.Claims.ToList());
        if (companyId.HasValue == false) return BadRequest();
        
        var requestDto = new PagedRequestDto
        {
            Page = page, 
            PageSize = pageSize,
            CompanyId = companyId.Value
        };

        var result = await _mediator.Send(new GetAllTeamsPagedQuery(requestDto));

        if (result.Success) return Ok(result.Data);

        return StatusCode(500, result.ErrorsList);
    }

    private int? GetCompanyId(List<Claim> claims)
    {
        var claim = claims.Where(claim => claim.Type == "CompanyId").FirstOrDefault();
        return int.Parse(claim.Value);
    }
}