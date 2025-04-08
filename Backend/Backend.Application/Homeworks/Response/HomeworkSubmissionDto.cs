using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Application.Homeworks.Response;

public class HomeworkSubmissionDto
{
    public int Id { get; set; } // probably StudentHomework ID
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public float? Grade { get; set; }
}