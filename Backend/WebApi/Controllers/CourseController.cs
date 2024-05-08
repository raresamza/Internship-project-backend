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
    public async Task<ActionResult> GetAllCourses()
    {
        //_service.SendEmail("Hello email");
        return Ok(await _mediator.Send(new GetCourses()));
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
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCourse(int id, Course course)
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
