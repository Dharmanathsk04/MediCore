using HealthcareSaaS.Application.Features.Appointments.Commands;
using HealthcareSaaS.Application.Features.Appointments.DTOs;
using HealthcareSaaS.Application.Features.Appointments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareSaaS.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetAppointmentByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (!result.Succeeded)
            return NotFound(result.Errors);

        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
    {
        var command = new CreateAppointmentCommand { AppointmentDto = dto };
        var result = await _mediator.Send(command);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }
}
