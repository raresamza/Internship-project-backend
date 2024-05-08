using Backend.Application.Classrooms.Create;
using Backend.Application.Classrooms.Delete;
using Backend.Application.Classrooms.Queries;
using Backend.Application.Classrooms.Response;
using Backend.Application.Classrooms.Update;
using Backend.Application.Courses.Create;
using Backend.Application.Courses.Delete;
using Backend.Application.Courses.Queries;
using Backend.Application.Courses.Response;
using Backend.Application.Courses.Update;
using Backend.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassroomController : ControllerBase
{
    private readonly IEmailSenderService _service;
    private readonly IMediator _mediator;

    public ClassroomController(IEmailSenderService service, IMediator mediator)
    {
        _service = service;
        _mediator = mediator;
    }


    [HttpGet]
    public async Task<ActionResult> GetAllClassroms()
    {
        //_service.SendEmail("Hello email");
        return Ok(await _mediator.Send(new GetClassrooms()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClassroom(int id)
    {
        return Ok(await _mediator.Send(new GetClassroomById(id)));
    }
    [HttpPost]
    public async Task<IActionResult> PostClassroom(ClassroomCreationDto classroom)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(await _mediator.Send(new CreateClassroom(classroom.Name, classroom.SchoolId)));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutClassroom(int id, Classroom classroom)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(await _mediator.Send(new UpdateClassroom(id, classroom)));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClassroom(int id)
    {
        return Ok(await _mediator.Send(new DeleteClassroom(id)));
    }
}
