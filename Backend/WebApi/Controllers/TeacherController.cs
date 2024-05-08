using Backend.Application.Teachers.Create;
using Backend.Application.Teachers.Delete;
using Backend.Application.Teachers.Queries;
using Backend.Application.Teachers.Responses;
using Backend.Application.Teachers.Update;
using Backend.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class TeacherController : ControllerBase
{

    private readonly IMediator _mediator;

    public TeacherController(IMediator mediator)
    {
        _mediator = mediator;
    }  

    [HttpGet]
    public async Task<ActionResult> GetAllTeachers()
    {
        return Ok(await _mediator.Send(new GetTeachers()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetTeacher(int id)
    {
        return Ok(await _mediator.Send(new GetTeacherById(id)));
    }
    [HttpPost]
    public async Task<IActionResult> PostTeacher(TeacherCreationDto teacher)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(await _mediator.Send(new CreateTeacher(teacher.Age,teacher.PhoneNumber,teacher.Name,teacher.Address,teacher.Subject)));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTeacher(int id, Teacher teacher)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(await _mediator.Send(new UpdateTeacher(id,teacher)));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeacher(int id)
    {
        return Ok(await _mediator.Send(new DeleteTeacher(id)));
    }
}
