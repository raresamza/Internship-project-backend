using Backend.Application.Schools.Create;
using Backend.Application.Schools.Delete;
using Backend.Application.Schools.Queries;
using Backend.Application.Schools.Response;
using Backend.Application.Schools.Update;
using Backend.Application.Students.Create;
using Backend.Application.Students.Delete;
using Backend.Application.Students.Queries;
using Backend.Application.Students.Update;
using Backend.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class SchoolController : ControllerBase
{
    private readonly IMediator _mediator;

    public SchoolController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllSchools(int pageNumber = 1, int pageSize = 10)
    {
        var query = new GetSchools(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetSchool(int id)
    {
        return Ok(await _mediator.Send(new GetSchoolById(id)));
    }
    [HttpPost]
    public async Task<IActionResult> PostSchool(SchoolCreationDto school)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(await _mediator.Send(new CreateSchool(school.Name)));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSchool(int id, School school)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
            return Ok(await _mediator.Send(new UpdateSchool(id, school)));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchool(int id)
    {
        return Ok(await _mediator.Send(new DeleteSchool(id)));
    }
}
