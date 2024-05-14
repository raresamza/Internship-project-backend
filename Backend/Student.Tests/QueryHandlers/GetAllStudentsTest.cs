using AutoMapper;
using Backend.Application.Abstractions;
using Backend.Application.Students.Queries;
using Backend.Application.Students.Responses;
using Microsoft.Extensions.Logging;
using Moq;
using Backend.Domain.Models;

namespace School.Tests.QueryHandlers;

public class GetAllStudentsTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<GetStudentsHandler>> _mockLogger;

    public GetAllStudentsTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<GetStudentsHandler>>();
    }

    [Fact]
    public async Task GetAllStudents_ShouldReturnListOfStudents()
    {
        // Arrange
        var students = new List<Student>
    {
        new Student { ID = 1, Name = "John", Age = 20, Address = "123 Street", ParentName = "Parent1", ParentEmail = "parent1@example.com", PhoneNumber = 123456789 },
        new Student { ID = 2, Name = "Alice", Age = 22, Address = "456 Avenue", ParentName = "Parent2", ParentEmail = "parent2@example.com", PhoneNumber = 987654321 }
    };

        _mockUnitOfWork.Setup(uow => uow.StudentRepository.GetAll()).ReturnsAsync(students);

        var expectedStudentDtos = students.Select(s => new StudentDto
        {
            ID = s.ID,
            Name = s.Name,
            Age = s.Age,
            Address = s.Address,
            ParentName = s.ParentName,
            ParentEmail = s.ParentEmail,
            PhoneNumber = s.PhoneNumber
        }).ToList();

        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<StudentDto>>(It.IsAny<IEnumerable<Student>>()))
    .Returns((IEnumerable<Student> students) =>
    {
        var matchingStudentDtos = students
            .Select(student => expectedStudentDtos.FirstOrDefault(dto => dto.ID == student.ID))
            .ToList();
        return matchingStudentDtos;
    });

        var handler = new GetStudentsHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);

        // Act
        var actualResult = await handler.Handle(new GetStudents(), default);

        // Assert
        Assert.NotNull(actualResult);
        Assert.Equal(expectedStudentDtos.Count, actualResult.TotalCount);
        Assert.Equal(expectedStudentDtos, actualResult.Items.ToList());

        _mockUnitOfWork.Verify(uow => uow.StudentRepository.GetAll(), Times.Once());
    }


    [Fact]
    public async Task GetAllStudents_WhenNoStudentsExist_ShouldReturnEmptyList()
    {
        // Arrange
        var emptyList = new List<Student>();
        _mockUnitOfWork.Setup(uow => uow.StudentRepository.GetAll()).ReturnsAsync(emptyList);

        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<StudentDto>>(It.IsAny<IEnumerable<Student>>()))
       .Returns(new List<StudentDto>());

        var handler = new GetStudentsHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);

        // Act
        var actualResult = await handler.Handle(new GetStudents(), default);

        // Assert
        Assert.NotNull(actualResult);
        Assert.Empty(actualResult.Items.ToList());

        _mockUnitOfWork.Verify(uow => uow.StudentRepository.GetAll(), Times.Once());
    }

}
