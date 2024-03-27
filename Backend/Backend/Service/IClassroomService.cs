using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Service
{
    public interface IClassroomService
    {
        public void AddTeacher(int teacherID, List<Teacher> teachers, int classroomID, List<Classroom> classrooms);
        public void AssignCourse(int classroomID, List<Classroom> classrooms, int courseID);
        public void AddStudent(int studentID, List<Student> students, int classroomID, List<Classroom> classrooms);
        public void RemoveTeacher(int classrommID, int teacherID, List<Classroom> classrooms, List<Teacher> teachers);
        public void RemoveStudent(int classroomID, int studentID, List<Student> students, List<Classroom> classrooms);
    }
}
