using Backend.Application.Courses.Actions;
using Backend.Application.Courses.Create;
using Backend.Application.Courses.Delete;
using Backend.Application.Courses.Queries;
using Backend.Application.Courses.Response;
using Backend.Application.Courses.Update;
using Backend.Application.Students.Queries;
using Backend.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly IEmailSenderService _service;
    private readonly IMediator _mediator;

    public CourseController(IEmailSenderService service,IMediator mediator)
    {
        _service = service;
        _mediator = mediator;
    }


    [HttpGet]
    public async Task<ActionResult> GetAllCourses(int pageNumber = 1, int pageSize = 10)
    {
        var query = new GetCourses(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpGet("subject/{subject}")]
    public async Task<ActionResult> GetAllCoursesBySubject(Subject subject)
    {
        var query = new GetCorusesBySubject(subject);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourse(int id)
    {
        return Ok(await _mediator.Send(new GetCourseById(id)));
    }
    [HttpPost]
    public async Task<IActionResult> PostCourse(CourseCreationDto course)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(await _mediator.Send(new CreateCourse(course.Name,course.Subject)));
    }
    [HttpPost("enroll")]
    public async Task<IActionResult> EnrollStudent(int studentId, int courseId) 
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(await _mediator.Send(new EnrollIntoCourse(studentId,courseId)));
    }
    [HttpPost("add")]
    //Route("/add")
    public async Task<IActionResult> PostGrade(int grade,int studentId,int courseId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(await _mediator.Send(new AddGrade(studentId,courseId,grade)));
    }
    [HttpDelete("remove")]
    //Route("/add")
    public async Task<IActionResult> RemoveGrade(int grade, int studentId, int courseId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(await _mediator.Send(new RemoveGrade(studentId,courseId,grade)));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCourse(int id, CourseUpdateDto course)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(await _mediator.Send(new UpdateCourse(id,course)));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        return Ok(await _mediator.Send(new DeleteCourse(id)));
    }
}
