using Backend.Application.Students.Create;
using Backend.Application.Students.Delete;
using Backend.Application.Students.Queries;
using Backend.Application.Students.Responses;
using Backend.Application.Students.Update;
using Backend.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;

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
    public async Task<ActionResult> GetAllStudents()
    {
        return Ok(await _mediator.Send(new GetStudents()));
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

        return Ok(await _mediator.Send(new CreateStudent(student.ParentEmail,student.ParentName,student.Age,student.PhoneNumber,student.Name,student.Address)));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutStudent(int id,Student student)
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
