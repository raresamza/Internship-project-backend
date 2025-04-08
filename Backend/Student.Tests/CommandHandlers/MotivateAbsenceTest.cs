using AutoMapper;
using Backend.Application.Absences.Response;
using Backend.Application.Abstractions;
using Backend.Application.Courses.Response;
using Backend.Application.Students.Actions;
using Backend.Application.Students.Create;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using Backend.Exceptions.AbsenceException;
using Backend.Exceptions.CourseException;
using Backend.Exceptions.StudentException;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Tests.CommandHandlers;

public class MotivateAbsenceTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<MotivateAbsenceHandler>> _mockLogger;

    public MotivateAbsenceTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<MotivateAbsenceHandler>>();
    }

    [Fact]
    public async Task Handle_SuccessfulMotivationOfAbsence_ReturnsMappedStudentDto()
    {
        // Arrange
        var expectedStudentId = 1;
        var expectedAbsenceId = 1;
        var expectedCourseId = 1;
        var expectedStudent = new Student
        {
            ID = expectedStudentId,
            Name = "John Doe",
            Age = 18,
            ParentEmail = "parent@example.com",
            ParentName = "Parent Doe",
            PhoneNumber = 123456789,
            Address = "123 Main St"
        };
        var expectedAbsence = new Absence(date:DateTime.Now)
        {
            Id = expectedAbsenceId,
            Course = null,
            CourseId = 1,
            Date = DateTime.Now
        };
        var expectedCourse = new Course
        {
            ID = expectedCourseId,
            Name = expectedStudent.Name,
            Subject = Subject.MATH,
        };

        _mockUnitOfWork.Setup(uow => uow.StudentRepository.GetById(expectedStudentId))
                       .ReturnsAsync(expectedStudent);
        _mockUnitOfWork.Setup(uow => uow.AbsenceRepository.GetById(expectedAbsenceId))
                       .ReturnsAsync(expectedAbsence);
        _mockUnitOfWork.Setup(uow => uow.CourseRepository.GetById(expectedCourseId))
                       .ReturnsAsync(expectedCourse);
        _mockUnitOfWork.Setup(uow => uow.StudentRepository.UpdateStudent(
                                  It.IsAny<StudentUpdateDto>(),
                                  It.IsAny<int>()))
                       .ReturnsAsync(expectedStudent);

        _mockMapper.Setup(mapper => mapper.Map<StudentDto>(It.IsAny<Student>()))
                   .Returns(new StudentDto()
                   {
                       ID = 1,
                       Name = "John Doe",
                       Age = 18,
                       ParentEmail = "parent@example.com",
                       ParentName = "Parent Doe",
                       PhoneNumber = 123456789,
                       Address = "123 Main St"
                   });

        _mockMapper.Setup(mapper => mapper.Map<AbsenceDto>(It.IsAny<Absence>()))
                   .Returns(new AbsenceDto
                   {
                       Id = expectedAbsenceId,
                       Course = null,
                       Date = DateTime.Now
                   });
        _mockMapper.Setup(mapper => mapper.Map<CourseDto>(It.IsAny<Course>()))
           .Returns(new CourseDto
           {
               ID = expectedCourseId,
               Name = expectedCourse.Name,
               Subject = Subject.MATH,
           });

        var handler = new MotivateAbsenceHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        var command = new MotivateAbsence(expectedStudentId, expectedAbsenceId, expectedCourseId);

        // Act
        var actualResult = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(actualResult);
        Assert.Equal(expectedStudent.ID, actualResult.ID);
        Assert.Equal(expectedStudent.Name, actualResult.Name);
        Assert.Equal(expectedStudent.ParentName, actualResult.ParentName);
        Assert.Equal(expectedStudent.Age, actualResult.Age);
        Assert.Equal(expectedStudent.Address, actualResult.Address);
        Assert.Equal(expectedStudent.PhoneNumber, actualResult.PhoneNumber);
        _mockUnitOfWork.Verify(uow => uow.StudentRepository.GetById(expectedStudentId), Times.Once());
        _mockUnitOfWork.Verify(uow => uow.AbsenceRepository.GetById(expectedAbsenceId), Times.Once());
        _mockUnitOfWork.Verify(uow => uow.CourseRepository.GetById(expectedCourseId), Times.Once());
        _mockUnitOfWork.Verify(uow => uow.StudentRepository.UpdateStudent(
            It.Is<StudentUpdateDto>(s => s.Name == expectedStudent.Name),
            expectedStudentId),
            Times.Once());
    }

    [Fact]
    public async Task Handle_StudentNotFound_ThrowsStudentNotFoundException()
    {
        // Arrange
        var expectedStudentId = 1;
        var expectedAbsenceId = 1;
        var expectedCourseId = 1;

        _mockUnitOfWork.Setup(uow => uow.StudentRepository.GetById(expectedStudentId))
                       .ReturnsAsync((Student)null);
        _mockUnitOfWork.Setup(uow => uow.AbsenceRepository.GetById(expectedAbsenceId))
                       .ReturnsAsync(new Absence(date: DateTime.Now));
        _mockUnitOfWork.Setup(uow => uow.CourseRepository.GetById(expectedCourseId))
                       .ReturnsAsync(new Course()
                       {
                           ID = expectedCourseId,
                           Name = "Math I",
                           Subject = Subject.MATH,
                       });

        var handler = new MotivateAbsenceHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        var command = new MotivateAbsence(expectedStudentId, expectedAbsenceId, expectedCourseId);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<StudentNotFoundException>(() => handler.Handle(command, CancellationToken.None));
        Assert.Equal($"Student with id: {expectedStudentId} was not found", exception.Message);

        _mockUnitOfWork.Verify(uow => uow.StudentRepository.GetById(expectedStudentId), Times.Once());
    }
    [Fact]
    public async Task Handle_AbsenceNotFound_ThrowsInvalidAbsenceException()
    {
        // Arrange
        var expectedStudentId = 1;
        var expectedAbsenceId = 1;
        var expectedCourseId = 1;
        var expectedStudent = new Student
        {
            ID = expectedStudentId,
            Name = "John Doe",
            Age = 18,
            ParentEmail = "parent@example.com",
            ParentName = "Parent Doe",
            PhoneNumber = 123456789,
            Address = "123 Main St"
        };

        _mockUnitOfWork.Setup(uow => uow.StudentRepository.GetById(expectedStudentId))
                       .ReturnsAsync(expectedStudent);
        _mockUnitOfWork.Setup(uow => uow.AbsenceRepository.GetById(expectedAbsenceId))
                       .ReturnsAsync((Absence)null); // Absence not found
        _mockUnitOfWork.Setup(uow => uow.CourseRepository.GetById(expectedCourseId))
                       .ReturnsAsync(new Course()
                       {
                           ID = expectedCourseId,
                           Name = "Math I",
                           Subject = Subject.MATH,
                       }); // Mock course object to avoid NullReferenceException

        var handler = new MotivateAbsenceHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        var command = new MotivateAbsence(expectedStudentId, expectedAbsenceId, expectedCourseId);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidAbsenceException>(() => handler.Handle(command, CancellationToken.None));
        Assert.Equal($"Absence with id: {expectedAbsenceId} was not found", exception.Message);

        _mockUnitOfWork.Verify(uow => uow.StudentRepository.GetById(expectedStudentId), Times.Once());
        _mockUnitOfWork.Verify(uow => uow.AbsenceRepository.GetById(expectedAbsenceId), Times.Once());
    }

    [Fact]
    public async Task Handle_CourseNotFound_ThrowsNullCourseException()
    {
        // Arrange
        var expectedStudentId = 1;
        var expectedAbsenceId = 1;
        var expectedCourseId = 1;
        var expectedStudent = new Student
        {
            ID = expectedStudentId,
            Name = "John Doe",
            Age = 18,
            ParentEmail = "parent@example.com",
            ParentName = "Parent Doe",
            PhoneNumber = 123456789,
            Address = "123 Main St"
        };
        var expectedAbsence = new Absence(date:DateTime.Now)
        {
            Id = expectedAbsenceId,
            Course = null,
            CourseId = expectedCourseId,
            Date = DateTime.Now
        };

        _mockUnitOfWork.Setup(uow => uow.StudentRepository.GetById(expectedStudentId))
                       .ReturnsAsync(expectedStudent);
        _mockUnitOfWork.Setup(uow => uow.AbsenceRepository.GetById(expectedAbsenceId))
                       .ReturnsAsync(expectedAbsence);
        _mockUnitOfWork.Setup(uow => uow.CourseRepository.GetById(expectedCourseId))
                       .ReturnsAsync((Course)null); // Course not found

        var handler = new MotivateAbsenceHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        var command = new MotivateAbsence(expectedStudentId, expectedAbsenceId, expectedCourseId);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NullCourseException>(() => handler.Handle(command, CancellationToken.None));
        Assert.Equal($"The course with id: {expectedCourseId} was not found", exception.Message);

        _mockUnitOfWork.Verify(uow => uow.StudentRepository.GetById(expectedStudentId), Times.Once());
        _mockUnitOfWork.Verify(uow => uow.AbsenceRepository.GetById(expectedAbsenceId), Times.Once());
        _mockUnitOfWork.Verify(uow => uow.CourseRepository.GetById(expectedCourseId), Times.Once());
    }
}