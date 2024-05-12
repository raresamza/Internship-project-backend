using AutoMapper;
using Backend.Application.Absences.Response;
using Backend.Application.Abstractions;
using Backend.Application.Students.Actions;
using Backend.Application.Students.Queries;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using Backend.Exceptions.AbsenceException;
using Backend.Exceptions.StudentException;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Tests.CommandHandlers;

public class AddAbsenceTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<AddAbsenceHandler>> _mockLogger;

    public AddAbsenceTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<AddAbsenceHandler>>();
    }

    [Fact]
    public async Task AddAbsence_WithValidStudentIdAndAbsenceId_ShouldAddAbsenceToStudent()
    {
        // Arrange
        var expectedStudentId = 1;
        var expectedAbsenceId = 1;
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

        var expectedAbsence = new Absence
        {
            Id = expectedAbsenceId,
            Course = null,
            CourseId = 1,
        };

        _mockUnitOfWork.Setup(uow => uow.StudentRepository.GetById(expectedStudentId))
                       .ReturnsAsync(expectedStudent);

        _mockUnitOfWork.Setup(uow => uow.AbsenceRepository.GetById(expectedAbsenceId))
                       .ReturnsAsync(expectedAbsence);

        _mockUnitOfWork.Setup(uow => uow.StudentRepository.UpdateStudent(
                                  It.IsAny<Student>(),
                                  It.IsAny<int>()))
                       .ReturnsAsync(expectedStudent);

        _mockMapper.Setup(mapper => mapper.Map<StudentDto>(It.IsAny<Student>()))
                   .Returns(new StudentDto
                   {
                       ID = expectedStudentId,
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
                   CourseName = string.Empty,
                   Date= DateTime.Now,
               });
        var handler = new AddAbsenceHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        var command = new AddAbsence(expectedStudentId, expectedAbsenceId);

        // Act
        var actualResult = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(actualResult);
        Assert.Equal(expectedStudentId, actualResult.ID);
        Assert.Equal(expectedStudent.Name, actualResult.Name);
        Assert.Equal(expectedStudent.ParentName, actualResult.ParentName);
        Assert.Equal(expectedStudent.Age, actualResult.Age);
        Assert.Equal(expectedStudent.Address, actualResult.Address);
        Assert.Equal(expectedStudent.PhoneNumber, actualResult.PhoneNumber);
        _mockUnitOfWork.Verify(uow => uow.StudentRepository.GetById(expectedStudentId), Times.Once());
        _mockUnitOfWork.Verify(uow => uow.AbsenceRepository.GetById(expectedAbsenceId), Times.Once());
        _mockUnitOfWork.Verify(uow => uow.StudentRepository.UpdateStudent(
            It.Is<Student>(s => s.ID == expectedStudentId),
            expectedStudentId),
            Times.Once());
    }

    [Fact]
    public async Task Handle_StudentNotFound_ThrowsStudentNotFoundException()
    {
        // Arrange
        var expectedStudentId = 1;
        var expectedAbsenceId = 1;
        var expectedAbsence = new Absence
        {
            Id = expectedAbsenceId,
            Course = null,
            CourseId = 1,
        };

        _mockUnitOfWork.Setup(uow => uow.StudentRepository.GetById(expectedStudentId))
                       .ReturnsAsync((Student)null);
        _mockUnitOfWork.Setup(uow => uow.AbsenceRepository.GetById(expectedAbsenceId))
                       .ReturnsAsync(expectedAbsence);

        _mockMapper.Setup(mapper => mapper.Map<StudentDto>(It.IsAny<Student>()))
                   .Returns(new StudentDto() { 
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
           CourseName = string.Empty,
           Date = DateTime.Now,
       });

        var handler = new AddAbsenceHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        var command = new AddAbsence(expectedStudentId, expectedAbsenceId);

        // Act & Assert
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        var exception = await Assert.ThrowsAsync<StudentNotFoundException>(act);
        Assert.Equal($"Student with id: {expectedStudentId} was not found", exception.Message);

        _mockUnitOfWork.Verify(uow => uow.StudentRepository.GetById(expectedStudentId), Times.Once());
    }

    [Fact]
    public async Task Handle_AbsenceNotFound_ThrowsInvalidAbsenceException()
    {
        // Arrange
        var expectedStudentId = 1;
        var expectedAbsenceId = 1;
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
                       .ReturnsAsync((Absence)null) ;

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
           CourseName = string.Empty,
           Date = DateTime.Now,
       });

        var handler = new AddAbsenceHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        var command = new AddAbsence(expectedStudentId, expectedAbsenceId);

        // Act & Assert
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        var exception = await Assert.ThrowsAsync<InvalidAbsenceException>(act);
        Assert.Equal($"Absence with id: {expectedStudentId} was not found", exception.Message);

        _mockUnitOfWork.Verify(uow => uow.StudentRepository.GetById(expectedStudentId), Times.Once());
    }

    [Fact]
    public async Task Handle_SuccessfulAdditionOfAbsence_ReturnsMappedStudentDto()
    {
        // Arrange
        var expectedStudentId = 1;
        var expectedAbsenceId = 1;
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
        var expectedAbsence = new Absence
        {
            Id = expectedAbsenceId,
            Course = null,
            CourseId = 1,
        };

        _mockUnitOfWork.Setup(uow => uow.StudentRepository.GetById(expectedStudentId))
                       .ReturnsAsync(expectedStudent);
        _mockUnitOfWork.Setup(uow => uow.AbsenceRepository.GetById(expectedAbsenceId))
                       .ReturnsAsync(expectedAbsence);
        _mockUnitOfWork.Setup(uow => uow.StudentRepository.UpdateStudent(
                                  It.IsAny<Student>(),
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
                       CourseName = string.Empty,
                       Date = DateTime.Now,
                   });

        var handler = new AddAbsenceHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        var command = new AddAbsence(expectedStudentId, expectedAbsenceId);

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
        _mockUnitOfWork.Verify(uow => uow.StudentRepository.UpdateStudent(
            It.Is<Student>(s => s.ID == expectedStudentId),
            expectedStudentId),
            Times.Once());
    }
}
