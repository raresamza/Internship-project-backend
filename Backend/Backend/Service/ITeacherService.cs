using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Service
{
    public interface ITeacherService
    {
        public void AddToClassroom(int classroomID, int teacherID, List<Teacher> teachers,List<Classroom> classrooms);
        public void AssignToCourse(int courseID, int teacherID, List<Teacher> teachers,List<Course> courses);

    }
}
