using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkLogger.Application._Commands.Tasks;
using WorkLogger.Domain.DTOs;
using WorkLogger.Domain.DTOs_responses;

namespace WorkLogger.Controllers;

[Authorize]
[ApiController]
[Route("api/tasks")]
public class TaskController(IMediator mediator) : ControllerBase
{
    private IMediator _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult> AddTask(AddTaskRequestDto dto)
    {
       var result = await _mediator.Send(new AddTaskCommand(dto));

       if (result.Success) return Created();

       return StatusCode(500, result.ErrorsList);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultResponseDto<UserTaskResponseDto>>> GetAllTasksPaged([FromQuery] int page, [FromQuery] int pageSize)
    {
        var companyId = GetCompanyId(User.Claims.ToList());
        if (companyId.HasValue == false) return BadRequest();
        
        var requestDto = new PagedRequestDto()
        {
            CompanyId = companyId.Value,
            PageSize = pageSize,
            Page = page
        };
        
        var result = await _mediator.Send(new GetAllCompanyTasksPagedQuery(requestDto));

        if (result.Success) return Ok(result.Data);

        return StatusCode(500, result.ErrorsList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserTaskDetailsResponseDto>> GetTaskDetails([FromRoute] int id)
    {
        var companyId = GetCompanyId(User.Claims.ToList());
        if (companyId.HasValue == false) return BadRequest();

        var result = await _mediator.Send(new GetTaskDetailsQuery(id));

        if (result.Success) return Ok(result.Data);

        return StatusCode(500, result.ErrorsList);
    }

    [HttpPatch()]
    public async Task<ActionResult> UpdateTask(UpdateTaskRequestDto dto)
    {
        var companyId = GetCompanyId(User.Claims.ToList());
        if (companyId.HasValue == false) return BadRequest();

        var result = await _mediator.Send(new UpdateTaskCommand(dto));

        if (result.Success) return Ok();

        return StatusCode(500, result.ErrorsList);
    }
    
    private int? GetCompanyId(List<Claim> claims)
    {
        var claim = claims.Where(claim => claim.Type == "CompanyId").FirstOrDefault();
        return int.Parse(claim.Value);
    }
}

