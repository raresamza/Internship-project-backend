using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Repository
{
    public interface ICourseRepository
    {
        public Course GetCourseById(int courseId, List<Course> courses);
    }
}
