using AutoMapper;
using Backend.Application.Students.Create;
using Backend.Application.Students.Responses;
using Backend.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;

namespace School.Tests.ControllerTests;

public class StudentControllerTest
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<IMapper> _mockMapper;

    public StudentControllerTest()
    {
        _mockMediator = new Mock<IMediator>();
        _mockMapper = new Mock<IMapper>();
    }
    [Fact]
    public async Task CreateStudent_ValidModelState_ReturnsOk()
    {
        // Arrange
        var controller = new StudentController(_mockMediator.Object);

        var studentDTto = new StudentCreationDto
        {
            ParentEmail = string.Empty,
            ParentName = string.Empty,
            PhoneNumber = 11111,
            Address = string.Empty,
            Age = 0,
            Name = string.Empty,
        };

        _mockMapper.Setup(mapper => mapper.Map<StudentDto>(It.IsAny<Student>()))
     .Returns(new StudentDto
     {
         ParentEmail = string.Empty,
         ParentName = string.Empty,
         PhoneNumber = 11111,
         Address = string.Empty,
         Age = 0,
         Name = string.Empty,
     });

        // Act
        var result = await controller.PostStudent(studentDTto);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        _mockMediator.Verify(m => m.Send(It.IsAny<CreateStudent>(), default), Times.Once);

    }
    [Fact]
    public async Task CreateStudent_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var controller = new StudentController(_mockMediator.Object);


        var studentDTto = new StudentCreationDto { Address = string.Empty, Age = 0, Name =string.Empty, ParentEmail= string.Empty, ParentName=string.Empty, PhoneNumber=0 };

        // Manually add model error to simulate invalid model state
        controller.ModelState.AddModelError(
            nameof(studentDTto.Name),
            "Required");

        // Act
        var result = await controller.PostStudent(studentDTto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        _mockMediator.Verify(m => m.Send(It.IsAny<CreateStudent>(), default), Times.Never);
    }
}
