using AutoMapper;
using Backend.Application.Catalogues.Response;
using Backend.Application.Courses.Response;
using Backend.Domain.Models;

namespace WebApi.Profiles.CatalogueProfiles;

public class CatalogueProfile : Profile
{

    public CatalogueProfile()
    {
        CreateMap<Catalogue, CatalogueDto>()
           .ForMember(dest => dest.Classroom, src => src.MapFrom(x => x.Classroom))
           .ForMember(dest => dest.ID, src => src.MapFrom(x => x.ID));
    }
}
