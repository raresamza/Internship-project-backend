using Backend.Application.Classrooms.Response;
using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Backend.Application.Catalogues.Response;

public class CatalogueDto
{
    [JsonIgnore]

    public Classroom Classroom { get; set; }
    //public ClassroomDto Classroom { get; set; }

    public int ID { get; set; }

    public static CatalogueDto FromCatalogue(Catalogue catalogue)
    {
        return new CatalogueDto 
        { 
            Classroom = catalogue.Classroom,
            //Classroom = ClassroomDto.FromClassroom(catalogue.Classroom),
            ID = catalogue.ID 
        };
    }

    public override string ToString()
    {
        return $"Catalogue(ID: {ID}) is for the following class:\n\n{Classroom}";
    }

}
