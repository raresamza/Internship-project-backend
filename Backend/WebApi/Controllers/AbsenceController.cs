using Backend.Application.Absences.Create;
using Backend.Application.Absences.Delete;
using Backend.Application.Absences.Queries;
using Backend.Application.Absences.Response;
using Backend.Application.Absences.Update;
using Backend.Application.Classrooms.Create;
using Backend.Application.Classrooms.Delete;
using Backend.Application.Classrooms.Queries;
using Backend.Application.Classrooms.Response;
using Backend.Application.Classrooms.Update;
using Backend.Application.Students.Queries;
using Backend.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AbsenceController : ControllerBase
{
    private readonly IEmailSenderService _service;
    private readonly IMediator _mediator;

    public AbsenceController(IEmailSenderService service, IMediator mediator, ILogger<AbsenceController> logger)
    {
        _service = service;
        _mediator = mediator;
    }


    [HttpGet]
    public async Task<ActionResult> GetAllAbsences(int pageNumber = 1, int pageSize = 10)
    {
        var query = new GetAbsences(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAbsence(int id)
    {
        return Ok(await _mediator.Send(new GetAbsenceById(id)));
    }
    [HttpPost]
    public async Task<IActionResult> PostAbsence(AbsenceCreationDto absence)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(await _mediator.Send(new CreateAbsence(absence.CourseId,absence.Date)));
    }

    //[HttpGet("{id}")]
    //public async Task<IActionResult> GetAbsenceByDateAndCourse(DateTime date, int courseId)
    //{
    //    return Ok(await _mediator.Send(new GetAbsenceByDateAndCourse(id)));
    //}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAbsence(int id, AbsenceUpdateDto absence)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(await _mediator.Send(new UpdateAbsence(id, absence)));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAbsence(int id)
    {
        return Ok(await _mediator.Send(new DeleteAbsence(id)));
    }
}
