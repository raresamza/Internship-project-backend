//using AutoMapper;
//using Backend.Application.Students.Create;
//using Backend.Application.Students.Delete;
//using Backend.Application.Students.Queries;
//using Backend.Application.Students.Responses;
//using Backend.Application.Students.Update;
//using Backend.Domain.Models;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using WebApi.Controllers;

//namespace School.Tests.ControllerTests;

//public class StudentControllerTest
//{
//    private readonly Mock<IMediator> _mockMediator;
//    private readonly Mock<IMapper> _mockMapper;

//    public StudentControllerTest()
//    {
//        _mockMediator = new Mock<IMediator>();
//        _mockMapper = new Mock<IMapper>();
//    }
//    [Fact]
//    public async Task CreateStudent_ValidModelState_ReturnsOk()
//    {
//        // Arrange
//        var controller = new StudentController(_mockMediator.Object);

//        var studentDTto = new StudentCreationDto
//        {
//            ParentEmail = string.Empty,
//            ParentName = string.Empty,
//            PhoneNumber = 11111,
//            Address = string.Empty,
//            Age = 0,
//            Name = string.Empty,
//        };

//        _mockMapper.Setup(mapper => mapper.Map<StudentDto>(It.IsAny<Student>()))
//     .Returns(new StudentDto
//     {
//         ParentEmail = string.Empty,
//         ParentName = string.Empty,
//         PhoneNumber = 11111,
//         Address = string.Empty,
//         Age = 0,
//         Name = string.Empty,
//     });

//        // Act
//        var result = await controller.PostStudent(studentDTto);

//        // Assert
//        Assert.IsType<OkObjectResult>(result);
//        _mockMediator.Verify(m => m.Send(It.IsAny<CreateStudent>(), default), Times.Once);

//    }
//    [Fact]
//    public async Task CreateStudent_InvalidModelState_ReturnsBadRequest()
//    {
//        // Arrange
//        var controller = new StudentController(_mockMediator.Object);


//        var studentDTto = new StudentCreationDto { Address = string.Empty, Age = 0, Name =string.Empty, ParentEmail= string.Empty, ParentName=string.Empty, PhoneNumber=0 };

//        // Manually add model error to simulate invalid model state
//        controller.ModelState.AddModelError(
//            nameof(studentDTto.Name),
//            "Required");

//        // Act
//        var result = await controller.PostStudent(studentDTto);

//        // Assert
//        Assert.IsType<BadRequestObjectResult>(result);
//        _mockMediator.Verify(m => m.Send(It.IsAny<CreateStudent>(), default), Times.Never);
//    }
//    [Fact]
//    public async Task GetStudentById_ValidStudentId_ReturnsOk()
//    {
//        // Arrange
//        var controller = new StudentController(_mockMediator.Object);

//        var expectedStudentId = 1;
//        var expectedStudentDto = new StudentDto
//        {
//            ID = expectedStudentId,
//            ParentEmail = "parent@example.com",
//            ParentName = "Parent Doe",
//            PhoneNumber = 123456789,
//            Address = "123 Main St",
//            Age = 18,
//            Name = "John Doe"
//        };

//        _mockMediator.Setup(m => m.Send(It.IsAny<GetStudentById>(), default))
//                     .ReturnsAsync(expectedStudentDto);

//        // Act
//        var result = await controller.GetStudent(expectedStudentId);

//        // Assert
//        var okResult = Assert.IsType<OkObjectResult>(result);
//        var actualStudent = Assert.IsType<StudentDto>(okResult.Value);

//        Assert.Equal(expectedStudentId, actualStudent.ID);
//        Assert.Equal(expectedStudentDto.Name, actualStudent.Name);
//        Assert.Equal(expectedStudentDto.ParentName, actualStudent.ParentName);
//        Assert.Equal(expectedStudentDto.Age, actualStudent.Age);
//        Assert.Equal(expectedStudentDto.Address, actualStudent.Address);
//        Assert.Equal(expectedStudentDto.PhoneNumber, actualStudent.PhoneNumber);
//        Assert.Equal(expectedStudentDto.ParentEmail, actualStudent.ParentEmail);

//        _mockMediator.Verify(m => m.Send(It.Is<GetStudentById>(q => q.studentId == expectedStudentId), default), Times.Once);
//    }
//    [Fact]
//    public async Task GetAllStudents_ReturnsOk()
//    {
//        // Arrange
//        var controller = new StudentController(_mockMediator.Object);

//        var expectedStudents = new List<StudentDto>
//    {
//        new StudentDto
//        {
//            ID = 1,
//            ParentEmail = "parent1@example.com",
//            ParentName = "Parent One",
//            PhoneNumber = 123456789,
//            Address = "123 Main St",
//            Age = 18,
//            Name = "John Doe"
//        },
//        new StudentDto
//        {
//            ID = 2,
//            ParentEmail = "parent2@example.com",
//            ParentName = "Parent Two",
//            PhoneNumber = 987654321,
//            Address = "456 Elm St",
//            Age = 19,
//            Name = "Jane Doe"
//        }
//    };

//        var expectedPaginatedResult = new PaginatedResult<StudentDto>
//        (
//            pageNumber: 1,
//            pageSize: 10,
//            totalCount: 2,
//            items: expectedStudents
//        );

//        _mockMediator.Setup(m => m.Send(It.IsAny<GetStudents>(), default))
//                     .ReturnsAsync(expectedPaginatedResult);

//        // Act
//        var result = await controller.GetAllStudents();

//        // Assert
//        var okResult = Assert.IsType<OkObjectResult>(result);
//        var actualPaginatedResult = Assert.IsType<PaginatedResult<StudentDto>>(okResult.Value);

//        Assert.Equal(expectedPaginatedResult.PageNumber, actualPaginatedResult.PageNumber);
//        Assert.Equal(expectedPaginatedResult.PageSize, actualPaginatedResult.PageSize);
//        Assert.Equal(expectedPaginatedResult.TotalCount, actualPaginatedResult.TotalCount);

//        var actualStudents = actualPaginatedResult.Items.ToList();
//        Assert.Equal(expectedStudents.Count, actualStudents.Count);
//        Assert.Equal(expectedStudents[0].ID, actualStudents[0].ID);
//        Assert.Equal(expectedStudents[0].Name, actualStudents[0].Name);
//        Assert.Equal(expectedStudents[0].ParentName, actualStudents[0].ParentName);
//        Assert.Equal(expectedStudents[0].Age, actualStudents[0].Age);
//        Assert.Equal(expectedStudents[0].Address, actualStudents[0].Address);
//        Assert.Equal(expectedStudents[0].PhoneNumber, actualStudents[0].PhoneNumber);
//        Assert.Equal(expectedStudents[0].ParentEmail, actualStudents[0].ParentEmail);

//        Assert.Equal(expectedStudents[1].ID, actualStudents[1].ID);
//        Assert.Equal(expectedStudents[1].Name, actualStudents[1].Name);
//        Assert.Equal(expectedStudents[1].ParentName, actualStudents[1].ParentName);
//        Assert.Equal(expectedStudents[1].Age, actualStudents[1].Age);
//        Assert.Equal(expectedStudents[1].Address, actualStudents[1].Address);
//        Assert.Equal(expectedStudents[1].PhoneNumber, actualStudents[1].PhoneNumber);
//        Assert.Equal(expectedStudents[1].ParentEmail, actualStudents[1].ParentEmail);
//    }
//        //[Fact]
//        //public async Task PutStudent_ValidModelState_ReturnsOk()
//        //{
//        //    // Arrange
//        //    var studentId = 1;
//        //    var controller = new StudentController(_mockMediator.Object);

//        //    var studentUpdateDto = new StudentUpdateDto
//        //    {
//        //        ParentEmail = "newparent@example.com",
//        //        ParentName = "New Parent",
//        //        PhoneNumber = 987654321,
//        //        Address = "456 Elm St",
//        //        Age = 19,
//        //        Name = "Jane Doe"
//        //    };

//        //    _mockMediator.Setup(m => m.Send(It.IsAny<UpdateStudent>(), default))
//        //                 .ReturnsAsync(new StudentDto
//        //                 {
//        //                     ID = studentId,
//        //                     ParentEmail = "newparent@example.com",
//        //                     ParentName = "New Parent",
//        //                     PhoneNumber = 987654321,
//        //                     Address = "456 Elm St",
//        //                     Age = 19,
//        //                     Name = "Jane Doe"
//        //                 });

//        //    // Act
//        //    var result = await controller.PutStudent(studentId, studentUpdateDto);

//        //    // Assert
//        //    var okResult = Assert.IsType<OkObjectResult>(result);
//        //    var student = Assert.IsType<StudentDto>(okResult.Value);

//        //    Assert.Equal(studentId, student.ID);
//        //    Assert.Equal(studentUpdateDto.Name, student.Name);
//        //    Assert.Equal(studentUpdateDto.ParentName, student.ParentName);
//        //    Assert.Equal(studentUpdateDto.Age, student.Age);
//        //    Assert.Equal(studentUpdateDto.Address, student.Address);
//        //    Assert.Equal(studentUpdateDto.PhoneNumber, student.PhoneNumber);
//        //    Assert.Equal(studentUpdateDto.ParentEmail, student.ParentEmail);

//        //    _mockMediator.Verify(m => m.Send(It.IsAny<UpdateStudent>(), default), Times.Once);
//        //}
//        [Fact]
//    public async Task DeleteStudent_ValidStudentId_ReturnsOk()
//    {
//        // Arrange
//        var studentId = 1;
//        var controller = new StudentController(_mockMediator.Object);

//        _mockMediator.Setup(m => m.Send(It.IsAny<DeleteStudent>(), default))
//                     .ReturnsAsync(new StudentDto
//                     {
//                         ID = studentId,
//                         Name = "John Doe",
//                         Age = 18,
//                         ParentEmail = "parent@example.com",
//                         ParentName = "Parent Doe",
//                         PhoneNumber = 123456789,
//                         Address = "123 Main St"
//                     });

//        // Act
//        var result = await controller.DeleteStudent(studentId);

//        // Assert
//        var okResult = Assert.IsType<OkObjectResult>(result);
//        var student = Assert.IsType<StudentDto>(okResult.Value);

//        Assert.Equal(studentId, student.ID);
//        _mockMediator.Verify(m => m.Send(It.IsAny<DeleteStudent>(), default), Times.Once);
//    }

//}
