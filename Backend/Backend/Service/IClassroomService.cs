using Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Service
{
    public interface IClassroomService
    {
        public void AddTeacher(int teacherID, int classroomID);
        public void AssignCourse(int classroomID, int courseID);
        public void AddStudent(int studentID, int classroomID);
        public void RemoveTeacher(int classrommID, int teacherID);
        public void RemoveStudent(int classroomID, int studentID);
    }
}
