using Backend.Application.Absences.Create;
using Backend.Application.Absences.Delete;
using Backend.Application.Absences.Queries;
using Backend.Application.Absences.Response;
using Backend.Application.Absences.Update;
using Backend.Application.Catalogues.Create;
using Backend.Application.Catalogues.Delete;
using Backend.Application.Catalogues.Queries;
using Backend.Application.Catalogues.Response;
using Backend.Application.Catalogues.Update;
using Backend.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogueController : ControllerBase
{
    private readonly IEmailSenderService _service;
    private readonly IMediator _mediator;

    public CatalogueController(IEmailSenderService service, IMediator mediator)
    {
        _service = service;
        _mediator = mediator;
    }


    [HttpGet]
    public async Task<ActionResult> GetAllACatalogues()
    {
        //_service.SendEmail("Hello email");
        return Ok(await _mediator.Send(new GetCatalogues()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCatalogue(int id)
    {
        return Ok(await _mediator.Send(new GetCatalogueById(id)));
    }
    [HttpPost]
    public async Task<IActionResult> PostCatalogue(CatalogueCreationDto catalogue)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(await _mediator.Send(new CreateCatalogue(catalogue.ClassroomId)));
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCatalogue(int id, Catalogue catalogue)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(await _mediator.Send(new UpdateCatalogue(id, catalogue)));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCatalogue(int id)
    {
        return Ok(await _mediator.Send(new DeleteCatalogue(id)));
    }
}
