namespace Stchools.Tests;

using Backend.Domain.Models;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.StudentException;
using Backend.Exceptions.TeacherException;
using Moq;
using Backend.Application.Abstractions;

public class SchoolTests
{

    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    public SchoolTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
    }


    [Fact]
    public void SchoolAddClassroomAddsToClassroomsCollection()
    {
        // Arrange
        var school = new School() { Name = "" };
        var classroom = new Classroom() { Name = "" };

        // Act
        school.Classrooms.Add(classroom);

        // Assert
        Assert.Single(school.Classrooms);
        Assert.Contains(classroom, school.Classrooms);
    }

    [Fact]
    public void SchoolRemoveClassroomRemovesFromClassroomsCollection()
    {
        var school = new School() { Name = "" };
        var classroom1 = new Classroom() { Name = "" };
        var classroom2 = new Classroom() { Name = "" };
        school.Classrooms.Add(classroom1);
        school.Classrooms.Add(classroom2);

        school.Classrooms.Remove(classroom1);

        Assert.Multiple(() =>
        {
            Assert.Single(school.Classrooms);
            Assert.DoesNotContain(classroom1, school.Classrooms);
            Assert.Contains(classroom2, school.Classrooms);
        });
    }

    [Fact]
    public void SchoolConstructorWithNameSetsName()
    {
        string name = "Colegiul National Decebal Deva";

        var school = new School() { Name = name };

        Assert.Equal(name, school.Name);
    }

    [Fact]
    public void SchoolDefaultConstructorSetsNameToEmptyString()
    {
        var school = new School() { Name = "" };

        Assert.Equal("", school.Name);
    }

    [Fact]
    public void SchoolDefaultConstructorSetsClassroomsToEmptyList()
    {
        var school = new School() { Name = "" };

        Assert.Empty(school.Classrooms);
    }

    [Fact]
    public void AddClassroomValidClassroomAddsToSchool()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var classroom = new Classroom() { Name = "12B" };
        var school = new School() { Name = "Colegiul National Decebal Deva" };

        unitOfWorkMock.SetupGet(uow => uow.SchoolRepository).Returns(Mock.Of<ISchoolRepository>());
        unitOfWorkMock.Setup(uow => uow.SchoolRepository.AddClassroom(classroom, school))
                      .Callback((Classroom c, School s) => s.Classrooms.Add(c));

        unitOfWorkMock.Object.SchoolRepository.AddClassroom(classroom, school);

        Assert.Contains(classroom, school.Classrooms);
    }

    [Fact]
    public void AddClassroomNullClassroomThrowsException()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var school = new School() { Name = "Colegiul National Decebal Deva" };

        unitOfWorkMock.SetupGet(uow => uow.SchoolRepository).Returns(Mock.Of<ISchoolRepository>());
        unitOfWorkMock.Setup(uow => uow.SchoolRepository.AddClassroom(It.IsAny<Classroom>(), school))
                      .Throws(new NullClassroomException("Classroom is not valid"));

        Assert.Throws<NullClassroomException>(() => unitOfWorkMock.Object.SchoolRepository.AddClassroom(It.IsAny<Classroom>(), school));
    }

    [Fact]
    public void AddClassroomAlreadyRegisteredClassroomThrowsException()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var classroom = new Classroom() { Name = "12B" };
        var school = new School() { Name = "Colegiul National Decebal Deva" };
        school.Classrooms.Add(classroom);

        unitOfWorkMock.SetupGet(uow => uow.SchoolRepository).Returns(Mock.Of<ISchoolRepository>());
        unitOfWorkMock.Setup(uow => uow.SchoolRepository.AddClassroom(classroom, school))
                      .Throws(new ClassroomAlreadyRegisteredException("Classroom is already registered"));

        Assert.Throws<ClassroomAlreadyRegisteredException>(() => unitOfWorkMock.Object.SchoolRepository.AddClassroom(classroom, school));
    }

    [Fact]
    public void RemoveClassroomValidClassroomRemovesFromClassrooms()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var classroom = new Classroom() { Name = "12B" };
        var school = new School() { Name = "Colegiul National Decebal Deva" };
        school.Classrooms.Add(classroom);

        unitOfWorkMock.SetupGet(uow => uow.SchoolRepository).Returns(Mock.Of<ISchoolRepository>());
        unitOfWorkMock.Setup(uow => uow.SchoolRepository.RemoveClassroom(classroom, school))
                      .Callback((Classroom c, School s) => s.Classrooms.Remove(c));

        unitOfWorkMock.Object.SchoolRepository.RemoveClassroom(classroom, school);

        Assert.DoesNotContain(classroom, school.Classrooms);
    }

    [Fact]
    public void RemoveClassroomNullClassroomThrowsException()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var school = new School() { Name = "Colegiul National Decebal Deva" };

        unitOfWorkMock.SetupGet(uow => uow.SchoolRepository).Returns(Mock.Of<ISchoolRepository>());
        unitOfWorkMock.Setup(uow => uow.SchoolRepository.RemoveClassroom(It.IsAny<Classroom>(), school))
                      .Throws(new NullClassroomException("Classroom not found"));

        Assert.Throws<NullClassroomException>(() => unitOfWorkMock.Object.SchoolRepository.RemoveClassroom(It.IsAny<Classroom>(), school));
    }

    [Fact]
    public void RemoveClassroomNotRegisteredClassroomThrowsException()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var classroom = new Classroom() { Name = "12B" };
        var school = new School() { Name = "Colegiul National Decebal Deva" };

        unitOfWorkMock.SetupGet(uow => uow.SchoolRepository).Returns(Mock.Of<ISchoolRepository>());
        unitOfWorkMock.Setup(uow => uow.SchoolRepository.RemoveClassroom(classroom, school))
                      .Throws(new ClassroomNotRegisteredException("Classroom is not registered"));

        Assert.Throws<ClassroomNotRegisteredException>(() => unitOfWorkMock.Object.SchoolRepository.RemoveClassroom(classroom, school));
    }

    //[Fact]
    //public void GetLastIdNoSchoolsReturnsOne()
    //{
    //    var mockRepository = new Mock<ISchoolRepository>();
    //    mockRepository.Setup(repo => repo.GetLastId()).Returns(1);

    //    var result = mockRepository.Object.GetLastId();

    //    Assert.Equal(1, result);
    //}

    //[Fact]
    //public void GetLastIdExistingSchoolsReturnsNextId()
    //{
    //    var mockRepository = new Mock<ISchoolRepository>();

    //    mockRepository.Setup(repo => repo.Create(It.IsAny<School>()))
    //                  .Callback<School>(school =>
    //                  {
    //                      mockRepository.Setup(repo => repo.GetLastId()).Returns(school.ID);
    //                  });

    //    var result1 = mockRepository.Object.GetLastId();
    //    mockRepository.Object.Create(new School { ID = 1, Name = "Colegiul National Decebal Deva" });
    //    var result2 = mockRepository.Object.GetLastId();
    //    mockRepository.Object.Create(new School { ID = 3, Name = "Colegiul National Decebal Deva" });
    //    var result3 = mockRepository.Object.GetLastId();

    //    Assert.Equal(0, result1);
    //    Assert.Equal(1, result2);
    //    Assert.Equal(3, result3);
    //}

    [Fact]
    public async Task GetByIdExistingIdReturnsSchool()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var school = new School { ID = 1, Name = "Colegiul National Decebal Deva" };

        unitOfWorkMock.SetupGet(uow => uow.SchoolRepository).Returns(Mock.Of<ISchoolRepository>());
        unitOfWorkMock.Setup(uow => uow.SchoolRepository.GetById(1)).ReturnsAsync(school);

        var result = await unitOfWorkMock.Object.SchoolRepository.GetById(1);

        Assert.Equal(school, result);
    }

    [Fact]
    public async Task GetByIdNonExistentIdReturnsNull()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        unitOfWorkMock.SetupGet(uow => uow.SchoolRepository).Returns(Mock.Of<ISchoolRepository>());
        unitOfWorkMock.Setup(uow => uow.SchoolRepository.GetById(999)).ReturnsAsync((School)null);

        var result = await unitOfWorkMock.Object.SchoolRepository.GetById(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAddsSchoolToList()
    {
        var school = new School { Name = "Colegiul National Decebal Deva" };
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        unitOfWorkMock.SetupGet(uow => uow.SchoolRepository).Returns(Mock.Of<ISchoolRepository>());
        unitOfWorkMock.Setup(uow => uow.SchoolRepository.Create(school)).ReturnsAsync(school);

        var createdSchool = await unitOfWorkMock.Object.SchoolRepository.Create(school);

        Assert.NotNull(createdSchool);
    }

    //[Fact]
    //public async Task UpdateSchoolExistingSchoolUpdatesSuccessfully()
    //{
    //    var school = new School { Name = "Colegiul National Decebal Deva", ID = 1 };
    //    var updatedSchool = new School { Name = "Updated School", ID = 1 };
    //    var unitOfWorkMock = new Mock<IUnitOfWork>();

    //    unitOfWorkMock.SetupGet(uow => uow.SchoolRepository).Returns(Mock.Of<ISchoolRepository>());
    //    unitOfWorkMock.Setup(uow => uow.SchoolRepository.Update(school.ID, updatedSchool)).ReturnsAsync(updatedSchool);
    //    unitOfWorkMock.Setup(uow => uow.SchoolRepository.GetById(school.ID)).ReturnsAsync(updatedSchool);

    //    await unitOfWorkMock.Object.SchoolRepository.Update(school.ID, updatedSchool);
    //    var retrievedSchool = await unitOfWorkMock.Object.SchoolRepository.GetById(school.ID);

    //    Assert.Equal(updatedSchool.Name, retrievedSchool?.Name);
    //    Assert.Equal(updatedSchool.Classrooms, retrievedSchool?.Classrooms);
    //}

    //[Fact]
    //public async Task UpdateSchoolNonExistingSchoolThrowsException()
    //{
    //    var repositoryMock = new Mock<ISchoolRepository>();
    //    var school = new School { ID = 1, Name = "Colegiul National Decebal Deva" };
    //    repositoryMock.Setup(repo => repo.Update(1, school))
    //                  .Throws(new TeacherNotFoundException("Teacher not found"));

    //    await Assert.ThrowsAsync<TeacherNotFoundException>(() => repositoryMock.Object.Update(1, school));
    //}

    [Fact]
    public async Task DeleteRemovesSchoolFromList()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var school = new School { ID = 1, Name = "Colegiul National Decebal Deva" };

        unitOfWorkMock.SetupGet(uow => uow.SchoolRepository).Returns(Mock.Of<ISchoolRepository>());
        unitOfWorkMock.Setup(uow => uow.SchoolRepository.Create(It.IsAny<School>())).ReturnsAsync(school);

        await unitOfWorkMock.Object.SchoolRepository.Create(school);
        await unitOfWorkMock.Object.SchoolRepository.Delete(school);

        var schools = await unitOfWorkMock.Object.SchoolRepository.GetAll();
        Assert.DoesNotContain(school, schools);
    }

    //[Fact]
    //public async Task UpdateExistingSchoolUpdatesSuccessfully()
    //{
    //    var school = new School { ID = 1, Name = "Colegiul National Decebal Deva" };
    //    var updatedSchool = new School { ID = 1, Name = "Updated School" };
    //    var unitOfWorkMock = new Mock<IUnitOfWork>();

    //    unitOfWorkMock.SetupGet(uow => uow.SchoolRepository).Returns(Mock.Of<ISchoolRepository>());
    //    unitOfWorkMock.Setup(uow => uow.SchoolRepository.Create(school)).ReturnsAsync(school);
    //    unitOfWorkMock.Setup(uow => uow.SchoolRepository.Update(1, updatedSchool)).ReturnsAsync(updatedSchool);

    //    await unitOfWorkMock.Object.SchoolRepository.Create(school);
    //    var result = await unitOfWorkMock.Object.SchoolRepository.Update(1, updatedSchool);

    //    Assert.Equal(updatedSchool, result);
    //}

    //[Fact]
    //public async Task UpdateNonExistingSchoolThrowsException()
    //{
    //    var repositoryMock = new Mock<ISchoolRepository>();
    //    var school = new School { ID = 1, Name = "Colegiul National Decebal Deva" };
    //    repositoryMock.Setup(repo => repo.Update(1, school))
    //                  .Throws(new StudentNotFoundException("Student not found"));

    //    await Assert.ThrowsAsync<StudentNotFoundException>(() => repositoryMock.Object.Update(1, school));
    //}
}