using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain.Models;
using Backend.Exceptions.StudentException;
using Backend.Application.Abstractions;

namespace Backend.Infrastructure;

public class StudentRepository : IStudentRepository
{

    //add private field list of students

    private readonly List<Student> _students = new();

    public void AddStudent(Student student)
    {
        _students.Add(student);
    }

    public void DeleteStudentById(int studentId)
    {
        _students.Remove(_students.First(student => student.ID == studentId));
    }

    public List<Student> GetAllStudents()
    {

        return _students;
    }

    //db mock
    public Student GetStudentById(int studentId)
    {
        if (!_students.Any(student => student.ID == studentId))
        {
            throw new StudentNotFoundException($"Student with ID: {studentId} not found!");
        }
        //return null;
        return _students.First(student => student.ID == studentId);
    }

    public void UpdateStudent(int studentID)
    {
        throw new NotImplementedException();
    }

    public Student Create(Student student)
    {
        _students.Add(student);
        return student;
    }
}
