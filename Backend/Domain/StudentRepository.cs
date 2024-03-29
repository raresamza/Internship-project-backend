using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain.Models;
using Backend.Exceptions.StudentException;
using Backend.Application.Abstractions;
using Backend.Exceptions.TeacherException;

namespace Backend.Infrastructure;

public class StudentRepository : IStudentRepository
{

    //add private field list of students

    private readonly List<Student> _students = new();

    public List<Student> GetAllStudents()
    {
        return _students;
    }

    //db mock

    public Student? GetById(int id)
    {
        return _students.FirstOrDefault(s => s.ID == id);
    }

    public Student Create(Student student)
    {
        _students.Add(student);
        return student;
    }

    public int GetLastId()
    {
        if (_students.Count == 0) return 1;
        var lastId = _students.Max(a => a.ID);
        return lastId + 1;
    }

    public void Delete(int id)
    {
        var student = _students.FirstOrDefault(s => s.ID == id);
        _students.Remove(student);
    }

    public void UpdateStudent(Student student, int id)
    {
        var oldStudent = GetById(id);
        if (oldStudent == null)
        {
            throw new TeacherNotFoundException($"Teacher with id {id} not found");
        }
        oldStudent = student;
    }
}
