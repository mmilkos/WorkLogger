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
        try
        {
            await _mediator.Send(new RegisterCompanyCommand(dto));
            return Ok();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}