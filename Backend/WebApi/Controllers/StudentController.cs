using Backend.Application.Abstractions;
using Backend.Application.Homeworks.Queries;
using Backend.Application.Students.Actions;
using Backend.Application.Students.Create;
using Backend.Application.Students.Delete;
using Backend.Application.Students.Queries;
using Backend.Application.Students.Responses;
using Backend.Application.Students.Update;
using Backend.Domain.Models;
using Backend.Infrastructure.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{

    private readonly IMediator _mediator;
    private readonly IMailService _mailService;

    public StudentController(IMediator mediator,IMailService mailService)
    {
        _mediator = mediator;
        _mailService = mailService;
    }



    [HttpGet("{id}/schedule")]
    public async Task<IActionResult> GetSchedule(int id)
    {
        var pdfBytes = await _mediator.Send(new GetStudentSchedule(id));

        var student = await _mediator.Send(new GetStudentById(id));
        var fileName = $"Schedule for ${student.Name}";
        return File(pdfBytes, "application/pdf", fileName);
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

    [HttpGet("by-email")]
    public async Task<ActionResult> GetStudentByEmail(string email) 
    {
        var query = new GetStudentByEmail(email);
        var result = await _mediator.Send(query);

        return Ok(result);

    }

    [HttpGet("{homeworkId}/submissions")]
    public async Task<IActionResult> GetSubmissionsForHomework(int homeworkId)
    {
        var submissions = await _mediator.Send(new GetSubmissionsForHomeworkQuery(homeworkId));
        return Ok(submissions);
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


    [HttpPost("{studentId}/send-grade-chart")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> SendGradeChart(int studentId, [FromForm(Name = "file")] IFormFile file)
    {
        var email = await _mediator.Send(new GetStudentEmailForChart(studentId));

        using var stream = file.OpenReadStream();
        var result = await _mailService.SendGradePdfAsync(email, stream);

        return result
            ? Ok("Grade chart sent to email.")
            : StatusCode(500, "Failed to send grade chart.");
    }

}
