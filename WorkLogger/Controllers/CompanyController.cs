using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkLogger.Application._Commands.Companies;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.Enums;

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

        switch (result.ErrorType)
        {
            case ErrorTypesEnum.BadRequest: return BadRequest(result.ErrorsList);
            default: return StatusCode(500, result.ErrorsList);
        }
    }
}