using Backend.Exceptions.StudentException;
using Backend.Domain.Models;
using Backend.Application.Abstractions;


namespace Backend.Infrastructure;
    public class CourseRepository : ICourseRepository
    {

        private readonly List<Course> _courses = new();

        public Course Create(Course course)
        {
            _courses.Add(course);
            return course;
        }

        //Db mock
        //public Course GetCourseById(int courseId, List<Course> courses)
        //{
        //    if (!_courses.Any(course => course.ID == courseId))
        //    {
        //        throw new StudentNotFoundException($"Student with ID: {courseId} not found!");
        //    }
        //    //return null;
        //    return _courses.First(course => course.ID == courseId);
        //}

        public Course GetCourseById(int courseId)
        {
            throw new NotImplementedException();
        }

    }
