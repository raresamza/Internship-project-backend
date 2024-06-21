using Backend.Application.Teachers.Responses;
using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Response;

public class TeacherClassroomDto
{
    public int TeacherId { get; set; }
    [JsonIgnore]
    public TeacherDto? Teacher { get; set; }
    public String? TeacherName { get; set; }

    //public Subject Subject { get; set; }
    public int ClassroomId { get; set; }
    //[JsonIgnore]
    //public ClassroomDto Classroom { get; set; }

    public static TeacherClassroomDto FromTeacherClassroom(TeacherClassroom teacherClassroom)
    {
        return new TeacherClassroomDto
        {
            TeacherName = teacherClassroom.Teacher?.Name,
            //Classroom = ClassroomDto.FromClassroom(teacherClassroom.Classroom),
            Teacher = TeacherDto.FromTeacher(teacherClassroom.Teacher),
            TeacherId = teacherClassroom.TeacherId,
            ClassroomId = teacherClassroom.ClassroomId,
            //Subject = teacherClassroom.Subject,
        };
    }
}
