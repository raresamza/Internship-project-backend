using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Repository
{
    public interface IStudentRepository
    {
        public Student GetStudentById(int studentId, List<Student> students);
        public void DeleteStudentById(int studentId);
        public List<Student> GetAllStudents();
        public void UpdateStudent(int studentID);
        public void AddStudent(Student student);

    }
}
