using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkLogger.Application._Commands.Teams;
using WorkLogger.Domain.DTOs;

namespace WorkLogger.Controllers;

[ApiController]
[Route("api/team")]
public class TeamController(IMediator mediator) : ControllerBase
{
    private IMediator _mediator = mediator;

    [HttpPost("create")]
    public async Task<ActionResult> CreateTeam(CreateTeamDto dto)
    {
        var companyId = GetCompanyId(User.Claims.ToList());
        if (companyId.HasValue == false) return BadRequest();

        dto.CompanyId = companyId.Value; 
        
        var result = await _mediator.Send(new CreateTeamCommand(dto));

        if (result.Success) return Created();
        
        return StatusCode(500, result.ErrorsList);
    }

    private int? GetCompanyId(List<Claim> claims)
    {
        var claim = claims.Where(claim => claim.Type == "CompanyId").FirstOrDefault();
        return int.Parse(claim.Value);
    }
}