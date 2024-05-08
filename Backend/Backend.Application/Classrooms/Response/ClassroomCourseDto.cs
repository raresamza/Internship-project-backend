using Backend.Application.Courses.Response;
using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Backend.Application.Classrooms.Response;

public class ClassroomCourseDto
{
    public int ClassroomId { get; set; }
    [JsonIgnore]
    public ClassroomDto Classroom { get; set; }

    public int? CourseId { get; set; }
    [JsonIgnore]
    public CourseDto? Course { get; set; }

    public static ClassroomCourseDto FromClassroomCourse(ClassroomCourse classroomCourse)
    {
        return new ClassroomCourseDto
        {
            ClassroomId = classroomCourse.ClassroomId,
            CourseId = classroomCourse.CourseId,
            //Classroom = ClassroomDto.FromClassroom(classroomCourse.Classroom),
            //Course = CourseDto.FromCourse(classroomCourse.Course)
        };
    }
}
