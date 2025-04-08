using Azure.Storage.Blobs;
using Backend.Application.Homeworks.Actions;
using Backend.Application.Homeworks.Queries;
using Backend.Application.Homeworks.Response;
using Backend.Application.Schools.Queries;
using Backend.Application.Students.Actions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class HomeworkController : ControllerBase
{


    private readonly IMediator _mediator;

    public HomeworkController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{studentId}/submit-homework/{homeworkId}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> SubmitHomework(
    int studentId,
    int homeworkId,
    [FromForm] FileUploadDto dto)
    {
        await _mediator.Send(new SubmitHomeworkFile(studentId, homeworkId, dto.File));
        return Ok("Homework submitted successfully.");
    }

    


    [HttpGet("download-homework")]
    public async Task<IActionResult> Download([FromQuery] int studentId, [FromQuery] int homeworkId)
    {
        var result = await _mediator.Send(new DownloadHomeworkFile(studentId, homeworkId));
        return result;
    }



    [HttpPost("grade")]
    public async Task<IActionResult> GradeHomework(
    [FromQuery] int StudentId,
    [FromQuery] int HomeworkId,
    [FromQuery] int grade)
    {
        var command = new GradeStudentHomework(StudentId, HomeworkId, grade);
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    [HttpGet]
    public async Task<ActionResult> GetAllHomeworks()
    {
        var query = new GetAllHomeworks();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetHomework(int id)
    {
        return Ok(await _mediator.Send(new GetHomeworkById(id)));
    }
}
