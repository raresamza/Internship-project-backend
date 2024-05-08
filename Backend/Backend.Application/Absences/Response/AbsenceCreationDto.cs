using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Absences.Response;

public class AbsenceCreationDto
{
    public required int CourseId { get; set; }
}
