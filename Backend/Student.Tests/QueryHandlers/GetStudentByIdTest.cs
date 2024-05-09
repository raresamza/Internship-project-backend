using AutoMapper;
using Backend.Application.Abstractions;
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

namespace School.Tests.QueryHandlers;

public class GetStudentByIdTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<GetStudentByIdHandler>> _mockLogger;

    public GetStudentByIdTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<GetStudentByIdHandler>>();
    }

    [Fact]
    public async Task GetStudentById_WithExistingStudentId_ShouldReturnStudent()
    {
        // Arrange
        var studentId = 1;
        var query = new GetStudentById(studentId);

        var expectedResult = new Student
        {
            Address = "deva",
            Age = 11,
            Name = "rares",
            ParentEmail = "mail@mail.com",
            ParentName = "Adi",
            PhoneNumber = 11111,
            ID = studentId
        };

        _mockUnitOfWork.Setup(uow => uow.StudentRepository.GetById(It.IsAny<int>())).ReturnsAsync(expectedResult);

        var expectedStudentDto = new StudentDto
        {
            Address = expectedResult.Address,
            Age = expectedResult.Age,
            Name = expectedResult.Name,
            ParentEmail = expectedResult.ParentEmail,
            ParentName = expectedResult.ParentName,
            PhoneNumber = expectedResult.PhoneNumber,
            ID = expectedResult.ID
        };

        _mockMapper.Setup(mapper => mapper.Map<StudentDto>(expectedResult)).Returns(expectedStudentDto);

        var handler = new GetStudentByIdHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);

        // Act
        var actualResult = await handler.Handle(query, default);

        // Assert
        Assert.NotNull(actualResult);
        Assert.Equal(expectedResult.ID, actualResult.ID);
        Assert.Equal(expectedResult.Address, actualResult.Address);
        Assert.Equal(expectedResult.Age, actualResult.Age);
        Assert.Equal(expectedResult.Name, actualResult.Name);
        Assert.Equal(expectedResult.ParentEmail, actualResult.ParentEmail);
        Assert.Equal(expectedResult.ParentName, actualResult.ParentName);
        Assert.Equal(expectedResult.PhoneNumber, actualResult.PhoneNumber);

        _mockUnitOfWork.Verify(uow => uow.StudentRepository.GetById(studentId), Times.Once());
    }
    [Fact]
    public async Task GetStudentById_WithNonExistingStudentId_ShouldReturnNull()
    {
        // Arrange
        var studentId = 12345; // Assuming this ID does not exist
        var query = new GetStudentById(studentId);

        _mockUnitOfWork.Setup(uow => uow.StudentRepository.GetById(It.IsAny<int>())).ReturnsAsync((Student)null);

        var handler = new GetStudentByIdHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);

        // Act
        var exception = await Assert.ThrowsAsync<ApplicationException>(async () => await handler.Handle(query, default));

        // Assert
        Assert.Equal("Student not found", exception.Message);

        _mockUnitOfWork.Verify(uow => uow.StudentRepository.GetById(studentId), Times.Once());
    }

}
