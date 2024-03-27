using Backend.Exceptions.StudentException;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Repository
{
    public class TeacherRepository : ITeacherRepository
    {
        //Db mock
        public void AddTeacher(Teacher student)
        {
            throw new NotImplementedException();
        }

        public void DeleteTeacherById(int studentId)
        {
            throw new NotImplementedException();
        }

        public List<Teacher> GetAllTeachers()
        {
            throw new NotImplementedException();
        }

        public Teacher GetTeacherById(int teacherId, List<Teacher> teachers)
        {
            if (!teachers.Any(teacher => teacher.ID == teacherId))
            {
                throw new StudentNotFoundException($"Student with ID: {teacherId} not found!");
            }
            return teachers.First(teacher => teacher.ID == teacherId);
        }

        public void UpdateTeacher(int studentID)
        {
            throw new NotImplementedException();
        }
    }
}
