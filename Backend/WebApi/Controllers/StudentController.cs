using Backend.Application.Students.Actions;
using Backend.Application.Students.Create;
using Backend.Application.Students.Delete;
using Backend.Application.Students.Queries;
using Backend.Application.Students.Responses;
using Backend.Application.Students.Update;
using Backend.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{

    private readonly IMediator _mediator;

    public StudentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllStudents(int pageNumber = 1, int pageSize = 10)
    {
        var query = new GetStudents(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("query")]
    public async Task<IActionResult> GetStudents(int page = 1, int pageSize = 10, string query = null)
    {
        if (!string.IsNullOrEmpty(query))
        {
            var queryCommand = new GetStudentsByName(query,page,pageSize);
            var result = await _mediator.Send(queryCommand);
            return Ok(new { students = result });
        }
        else
        {
            var queryCommand = new GetStudentsWithQuery(query, page, pageSize);
            var students = await _mediator.Send(queryCommand);
            return Ok(new { students});
        }
    }


    [HttpGet("all")]
    public async Task<ActionResult> GetAllStudentsNotPaginated()
    {
        var query = new GetAllStudents();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetStudent(int id)
    {
        return Ok(await _mediator.Send(new GetStudentById(id)));
    }
    [HttpPost]
    public async Task<IActionResult> PostStudent(StudentCreationDto student)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        //CreatedAtAction for all posts
        return Ok(await _mediator.Send(new CreateStudent(student.ParentEmail,student.ParentName,student.Age,student.PhoneNumber,student.Name,student.Address)));
    }
    [HttpPost("absence")]
    public async Task<IActionResult> AddAbsence(int studentId,int absenceId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(await _mediator.Send(new AddAbsence(studentId,absenceId)));
    }

    [HttpDelete("removeAbsence")]
    public async Task<IActionResult> DeleteStudent(int studentId,int absenceId,int courseId)
    {
        return Ok(await _mediator.Send(new MotivateAbsence(studentId,absenceId,courseId)));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutStudent(int id,StudentUpdateDto student)
    {
        if(!ModelState.IsValid )
        {
            return BadRequest(ModelState);
        }

        return Ok(await _mediator.Send(new UpdateStudent(id,student)));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        return Ok(await _mediator.Send(new DeleteStudent(id)));
    }
}
