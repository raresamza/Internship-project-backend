using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Students.Actions;
using Backend.Application.Students.Delete;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using Backend.Exceptions.StudentException;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Tests.CommandHandlers;

public class DeleteStundentTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<DeleteStudentHandler>> _mockLogger;

    public DeleteStundentTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<DeleteStudentHandler>>();
    }
    [Fact]
    public async Task Handle_StudentNotFound_ThrowsStudentNotFoundException()
    {
        // Arrange
        var expectedStudentId = 1;
        _mockUnitOfWork.Setup(uow => uow.StudentRepository.GetById(expectedStudentId))
                       .ReturnsAsync((Student)null); // Student not found

        var handler = new DeleteStudentHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        var command = new DeleteStudent(expectedStudentId);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<StudentNotFoundException>(() => handler.Handle(command, CancellationToken.None));
        Assert.Equal($"The student with id: {expectedStudentId} was not found", exception.Message);

        _mockUnitOfWork.Verify(uow => uow.StudentRepository.GetById(expectedStudentId), Times.Once());
    }
    [Fact]
    public async Task Handle_ExistingStudent_DeletesStudentSuccessfully()
    {
        // Arrange
        var expectedStudentId = 1;
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


        var handler = new DeleteStudentHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        var command = new DeleteStudent(expectedStudentId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedStudentId, result.ID);
        Assert.Equal(expectedStudent.Name, result.Name);
        Assert.Equal(expectedStudent.ParentName, result.ParentName);
        Assert.Equal(expectedStudent.Age, result.Age);
        Assert.Equal(expectedStudent.Address, result.Address);
        Assert.Equal(expectedStudent.PhoneNumber, result.PhoneNumber);

        _mockUnitOfWork.Verify(uow => uow.StudentRepository.GetById(expectedStudentId), Times.Once());
        _mockUnitOfWork.Verify(uow => uow.StudentRepository.Delete(expectedStudent), Times.Once());
    }

}
