using Backend.Application.Abstractions;
using Backend.Application.Catalogues.Response;
using Backend.Domain.Models;
using Backend.Exceptions.ClassroomException;
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
    //will remove with more unit of work implmentations
    private readonly IClassroomRepository _classroomRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateaCatalogueHandler(IClassroomRepository classroomRepository, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CatalogueDto> Handle(CreateCatalogue request, CancellationToken cancellationToken)
    {
        try
        {
            var classroom = await _unitOfWork.ClassroomRepository.GetById(request.classroomId);

            if (classroom == null)
            {
                throw new NullClassroomException($"The classroom wiht id: {request.classroomId} was not found");
            }

            var catalogue = new Catalogue() { Classroom = classroom };

            await _unitOfWork.BeginTransactionAsync();
            var newCatalogue = await _unitOfWork.CatalogueRepository.Create(catalogue);
            await _unitOfWork.CommitTransactionAsync();


            return CatalogueDto.FromCatalogue(newCatalogue);
        } catch (Exception ex)
        {
            await Console.Out.WriteLineAsync(ex.Message);
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
        
    }

}
