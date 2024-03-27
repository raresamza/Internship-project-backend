using Backend.Exceptions.StudentException;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Repository
{
    public class CourseRepository : ICourseRepository
    {
        //Db mock
        public Course GetCourseById(int courseId, List<Course> courses)
        {
            if (!courses.Any(course => course.ID == courseId))
            {
                throw new StudentNotFoundException($"Student with ID: {courseId} not found!");
            }
            //return null;
            return courses.First(course => course.ID == courseId);
        }
    }
}
