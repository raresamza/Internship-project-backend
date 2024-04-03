using Backend.Application.Abstractions;
using Backend.Application.Catalogues.Response;
using Backend.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Catalogues.Create;

public record CreateCatalogue(int classroomId) : IRequest<CatalogueDto>;
public class CreateaCatalogueHandler : IRequestHandler<CreateCatalogue, CatalogueDto>
{

    private readonly IClassroomRepository _classroomRepository;
    private readonly ICatalogueRepository _catalogueRepository;
    public CreateaCatalogueHandler(IClassroomRepository classroomRepository, ICatalogueRepository catalogueRepository)
    {
        _classroomRepository = classroomRepository;
        _catalogueRepository = catalogueRepository;
    }

    public Task<CatalogueDto> Handle(CreateCatalogue request, CancellationToken cancellationToken)
    {
        var classroom=_classroomRepository.GetById(request.classroomId);
        var catalogue = new Catalogue() { Classroom = classroom, ID =GetNextId()};
        var newCatalogue=_catalogueRepository.Create(catalogue);

        return Task.FromResult(CatalogueDto.FromCatalogue(newCatalogue));
    }

    private int GetNextId()
    {
        return _classroomRepository.GetLastId();
    }
}
