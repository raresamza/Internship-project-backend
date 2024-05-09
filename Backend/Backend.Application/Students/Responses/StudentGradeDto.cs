using Backend.Application.Courses.Response;
using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Backend.Application.Students.Responses;

    public class StudentGradeDto
    {
        //public int StudentId { get; set; }
        [JsonIgnore]
        public StudentDto? Student { get; set; }
        public int CourseId { get; set; }
        public String? CourseName { get; set; }
        [JsonIgnore]
        public CourseDto? Course { get; set; }
        public List<int> GradeValues { get; set; } = new();


    public static StudentGradeDto FromStudentGrade(StudentGrade studentGrade)
    {
        return new StudentGradeDto
        {
            //StudentId = studentGrade.StudentId,
            CourseName = studentGrade.Course?.Name,
            //Student = StudentDto.FromStudent(studentGrade.Student),
            CourseId = studentGrade.CourseId,
            GradeValues = studentGrade.GradeValues,
            //Course = CourseDto.FromCourse(studentGrade.Course),
        };
    }
}
