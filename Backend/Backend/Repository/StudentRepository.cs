using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Exceptions.StudentException;
namespace Backend.Repository
{
    public class StudentRepository : IStudentRepository
    {
        public void AddStudent(Student student)
        {
            throw new NotImplementedException();
        }

        public void DeleteStudentById(int studentId)
        {
            throw new NotImplementedException();
        }

        public List<Student> GetAllStudents()
        {
            throw new NotImplementedException();
        }

        //db mock
        public Student GetStudentById(int studentId, List<Student> students)
        {
            if (!students.Any(student => student.ID == studentId))
            {
                throw new StudentNotFoundException($"Student with ID: {studentId} not found!");
            }
            return students.First(student => student.ID == studentId);
        }

        public void UpdateStudent(int studentID)
        {
            throw new NotImplementedException();
        }
    }
}
