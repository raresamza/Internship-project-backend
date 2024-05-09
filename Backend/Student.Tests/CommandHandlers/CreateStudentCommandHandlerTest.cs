using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Students.Create;
using Backend.Application.Students.Queries;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Tests.CommandHandlers;

public class CreateStudentCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<CreateStudentHandler>> _mockLogger;

    public CreateStudentCommandHandlerTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<CreateStudentHandler>>();
    }
    [Fact]
    public async Task CreateStudent_ValidCommand_ShouldCreateStudent()
    {
        // Arrange
        var expectedStudent = new Student
        {
            Address = "deva",
            Age = 11,
            Name = "rares",
            ParentEmail = "mail@mail.com",
            ParentName = "Adi",
            PhoneNumber = 11111,
            ID = 1
        };

        _mockUnitOfWork.Setup(uow => uow.StudentRepository.Create(It.IsAny<Student>())).ReturnsAsync(expectedStudent);

        var expectedStudentDto = new StudentDto
        {
            Address = expectedStudent.Address,
            Age = expectedStudent.Age,
            Name = expectedStudent.Name,
            ParentEmail = expectedStudent.ParentEmail,
            ParentName = expectedStudent.ParentName,
            PhoneNumber = expectedStudent.PhoneNumber,
            ID = expectedStudent.ID
        };

        _mockMapper.Setup(mapper => mapper.Map<StudentDto>(It.IsAny<Student>())).Returns(expectedStudentDto);

        var handler = new CreateStudentHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        var command = new CreateStudent(
            parentEmail: expectedStudent.ParentEmail,
            parentName: expectedStudent.ParentName,
            age: expectedStudent.Age,
            phoneNumber: expectedStudent.PhoneNumber,
            name: expectedStudent.Name,
            address: expectedStudent.Address);

        // Act
        var actualResult = await handler.Handle(command, default);

        // Assert
        Assert.NotNull(actualResult);
        Assert.Equal(expectedStudentDto.ID, actualResult.ID);
        Assert.Equal(expectedStudentDto.Name, actualResult.Name);
        Assert.Equal(expectedStudentDto.ParentEmail, actualResult.ParentEmail);
        Assert.Equal(expectedStudentDto.ParentName, actualResult.ParentName);
        Assert.Equal(expectedStudentDto.PhoneNumber, actualResult.PhoneNumber);
        Assert.Equal(expectedStudentDto.Address, actualResult.Address);
        Assert.Equal(expectedStudentDto.Age, actualResult.Age);

        _mockUnitOfWork.Verify(uow => uow.StudentRepository.Create(It.IsAny<Student>()), Times.Once());
        _mockMapper.Verify(mapper => mapper.Map<StudentDto>(It.IsAny<Student>()), Times.Once());
    }




}
