using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkLogger.Application._Commands.Companies;
using WorkLogger.Domain.DTOs;

namespace WorkLogger.Controllers;

[ApiController]
[Route("api/company")]
public class CompanyController(IMediator mediator) : ControllerBase
{
    private IMediator _mediator = mediator;

    [HttpPost("register")]
    public async Task<ActionResult> RegisterOrganization(RegisterCompanyDto dto)
    {
   
        var result = await _mediator.Send(new RegisterCompanyCommand(dto));
        
        if (result.Success) return Created();

        return StatusCode(500, result.ErrorsList);
    }
}