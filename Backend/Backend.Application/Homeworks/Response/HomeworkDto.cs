using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Backend.Domain.Models;

namespace Backend.Application.Homeworks.Response;

public class HomeworkDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [JsonIgnore]
    public DateTime Deadline { get; set; }
    public float? Grade { get; set; }

    public string DeadlineFormatted => Deadline.ToUniversalTime().ToString("R");

    public List<StudentHomeworkDto> StudentHomeworks { get; set; } = new();



    public static HomeworkDto FromHomework(Homework homework)
    {
        return new HomeworkDto
        {
            Id = homework.ID,
            Title = homework.Title,
            Description = homework.Description,
            Deadline = homework.Deadline.ToUniversalTime(),
            Grade =homework.Grade,
            StudentHomeworks = homework.StudentHomeworks
                .Select(StudentHomeworkDto.FromStudentHomework)
                .ToList()
        };
    }
}
