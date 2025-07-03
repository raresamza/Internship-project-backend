using System.Diagnostics;
using Backend.Application.Students.Queries;
using Backend.Application.Teachers.Actions;
using Backend.Application.Teachers.Create;
using Backend.Application.Teachers.Delete;
using Backend.Application.Teachers.Queries;
using Backend.Application.Teachers.Responses;
using Backend.Application.Teachers.Update;
using Backend.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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


    [Authorize(Roles = "Student,Teacher,Admin,Parent")]
    [HttpGet("{id}/schedule")]
    public async Task<IActionResult> GetSchedule(int id)
    {
        var pdf = await _mediator.Send(new GetTeacherSchedule(id));

        var teacher = await _mediator.Send(new GetTeacherById(id));
        var fileName = $"Schedule for ${teacher.Name}";
        return File(pdf, "application/pdf", fileName);
    }

    [Authorize(Roles = "Student,Teacher,Admin,Parent")]
    [HttpGet("{name}/name")]
    public async Task<ActionResult> GetTeacherByName(string name)
    {
        var query = new GetTeacherByName(name);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [Authorize(Roles = "Student,Teacher,Admin,Parent")]
    [HttpGet]
    public async Task<ActionResult> GetAllTeachers(int pageNumber = 1, int pageSize = 10)
    {
        var query = new GetTeachers(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [Authorize(Roles = "Student,Teacher,Admin,Parent")]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetTeacher(int id)
    {
        return Ok(await _mediator.Send(new GetTeacherById(id)));
    }

    [Authorize(Roles = "Teacher,Admin")]
    [HttpPost]
    public async Task<IActionResult> PostTeacher(TeacherCreationDto teacher)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(await _mediator.Send(new CreateTeacher(teacher.Age,teacher.PhoneNumber,teacher.Name,teacher.Address,teacher.Subject)));
    }

    [Authorize(Roles = "Teacher,Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTeacher(int id, TeacherUpdateDto teacher)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(await _mediator.Send(new UpdateTeacher(id,teacher)));
    }

    [Authorize(Roles = "Teacher,Admin")]
    [HttpPut("assign")]
    public async Task<IActionResult> AssignTeacherToCourse(int courseId,int teacherId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(await _mediator.Send(new AssignTeacherToCourse(courseId,teacherId)));
    }

    [Authorize(Roles = "Teacher,Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeacher(int id)
    {
        return Ok(await _mediator.Send(new DeleteTeacher(id)));
    }
}
