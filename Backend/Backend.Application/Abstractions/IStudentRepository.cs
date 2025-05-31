﻿using Backend.Application.Leaderboard.Responses;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;


namespace Backend.Application.Abstractions;

public interface IStudentRepository
{

    public Task<Student> Create(Student student);
    Task<List<Student>> GetByNames(string name);
    Task<List<Student>> GetWithQuery(string query, int page, int pageSize);
    Task<int> GetTotalCount(string query);
    public Task<List<Student>> GetAll();
    //int GetLastId();
    public void MotivateAbsence(DateTime date, Course course, Student student);
    public Task<Student?> GetById(int id);

    public Task<Student?> GetByEmail(string email);

    public Task<List<StudentHomework>> GetSubmissionsByHomeworkId(int homeworkId);



    public void SubmitHomework(StudentHomework studentHomework);

    public void GradeHomework(StudentHomework studentHomework, int grade);
    public Task<StudentHomework?> GetByStudentAndHomework(int studentId, int homeworkId);
    public Task<Student?> GetByName(string name); 
    public Task Delete(Student student);
    public void RemoveGrade(Student student, Course course, int grade);
    public void AddAbsence(Student student,Absence absence);
    public Task<Student> UpdateStudent(StudentUpdateDto student,int id);
    public void AddGrade(int grade, Student s, Course c);
    public void EnrollIntoCourse(Student s, Course c);
    public Task<IEnumerable<Student>> GetAllWithGradesAsync();
    public Task<IEnumerable<ClassStudentLeaderboardDto>> GetLeaderboardData();
}
