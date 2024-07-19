using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkLogger.Application._Commands.Tasks;
using WorkLogger.Domain.DTOs;

namespace WorkLogger.Controllers;

[ApiController]
[Route("api/task")]
public class TaskController(IMediator mediator) : ControllerBase
{
    private IMediator _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult> AddTask(AddTaskRequestDto dto)
    {
       var result = await _mediator.Send(new AddTaskCommand(dto));

       if (result.Success) return Ok();

       return StatusCode(500, result.ErrorsList);
    }
}