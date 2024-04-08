namespace Student.Tests;

using Backend.Domain.Models;
using Backend.Exceptions.ClassroomException;
using Backend.Exceptions.Placeholders;
using Backend.Exceptions.StudentException;
using Backend.Exceptions.TeacherException;
using Backend.Infrastructure;
using Moq;
using Backend.Application.Abstractions;
using Xunit.Abstractions;

public class StudentTests
{

    //    private readonly Student _student;
    //    private readonly Student _student1;

    //    public StudentTests()
    //    {
    //        _student= new Student("mail@mail.com", "Adi", 11, 11111111, "Rares", "deva");
    //    }

    //can add assert false
    //    [Theory]
    //    [InlineData("Adi")]
    //    public void Constructor_ParentNameTest(string expectedParentName)
    //    {
    //        Assert.Equal(expectedParentName, _student.ParentName);
    //    }

    //    [Theory]
    //    [InlineData("mail@mail.com")]
    //    public void Constructor_ParentEmailTest(string expectedParentEmail)
    //    {
    //        Assert.Equal(expectedParentEmail, _student.ParentEmail);
    //    }

    //    [Theory]
    //    [InlineData(11)]
    //    public void Constructor_AgeTest(int expectedAge)
    //    {
    //        Assert.Equal(expectedAge, _student.Age);
    //    }

    //    [Theory]
    //    [InlineData(11111111)]
    //    public void Constructor_PhoneNumberTest(int expectedPhoneNumber)
    //    {

    //        Assert.Equal(expectedPhoneNumber, _student.PhoneNumber);
    //    }

    //    [Theory]
    //    [InlineData("Rares")]
    //    public void Constructor_NameTest(string expectedName)
    //    {
    //        Assert.Equal(expectedName, _student.Name);
    //    }

    //    [Theory]
    //    [InlineData("deva")]
    //    public void Constructor_AddressTest(string expectedAddress)
    //    {
    //        Assert.Equal(expectedAddress, _student.Address);
    //    }

    [Fact]
    public void School_AddClassroom_AddsToClassroomsCollection()
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
    public void School_RemoveClassroom_RemovesFromClassroomsCollection()
    {
        var school = new School() { Name = "" };
        var classroom1 = new Classroom() { Name = "" };
        var classroom2 = new Classroom() { Name = "" };
        school.Classrooms.Add(classroom1);
        school.Classrooms.Add(classroom2);

        school.Classrooms.Remove(classroom1);

        Assert.Multiple(() =>
        {
            Assert.Equal(1, school.Classrooms.Count);
            Assert.DoesNotContain(classroom1, school.Classrooms);
            Assert.Contains(classroom2, school.Classrooms);
        });
    }

    public void School_ConstructorWithName_SetsName()
    {
        string name = "Colegiul National Decebal Deva";

        var school = new School() { Name=name};

        Assert.Equal(name, school.Name);
    }

    [Fact]
    public void School_DefaultConstructor_SetsNameToEmptyString()
    {
        var school = new School() { Name = ""};

        Assert.Equal("", school.Name);
    }

    [Fact]
    public void School_DefaultConstructor_SetsClassroomsToEmptyList()
    {
        var school = new School() { Name = "" };

        Assert.Empty(school.Classrooms);
    }

    [Fact]
    public void AddClassroom_ValidClassroom_AddsToSchool()
    {
        var repository = new SchoolRepository();
        var classroom = new Classroom() { Name = "12B" };
        var school = new School() { Name = "Colegiul National Decebal Deva"};

        repository.AddClassroom(classroom, school);

        Assert.Contains(classroom, school.Classrooms);
    }

    [Fact]
    public void AddClassroom_NullClassroom_ThrowsException()
    {
        var repository = new SchoolRepository();
        var school = new School() { Name = "Colegiul National Decebal Deva" };

        Assert.Throws<NullClassroomException>(() => repository.AddClassroom(null, school));
    }

    [Fact]
    public void AddClassroom_AlreadyRegisteredClassroom_ThrowsException()
    {
        var repository = new SchoolRepository();
        var classroom = new Classroom() { Name = "12B" };
        var school = new School() { Name = "Colegiul National Decebal Deva" };
        school.Classrooms.Add(classroom);

        Assert.Throws<ClassroomAlreadyRegisteredException>(() => repository.AddClassroom(classroom, school));
    }

    [Fact]
    public void RemoveClassroom_ValidClassroom_RemovesFromClassrooms()
    {
        var repository = new SchoolRepository();
        var classroom = new Classroom() { Name = "12B" };
        var school = new School() { Name = "Colegiul National Decebal Deva" };
        school.Classrooms.Add(classroom);

        repository.RemoveClassroom(classroom, school);

        Assert.DoesNotContain(classroom, school.Classrooms);
    }

    [Fact]
    public void RemoveClassroom_NullClassroom_ThrowsException()
    {
        var repository = new SchoolRepository();
        var school = new School() { Name = "Colegiul National Decebal Deva" };

        Assert.Throws<NullClassroomException>(() => repository.RemoveClassroom(null, school));
    }

    [Fact]
    public void RemoveClassroom_NotRegisteredClassroom_ThrowsException()
    {
        var repository = new SchoolRepository();
        var classroom = new Classroom() { Name = "12B" };
        var school = new School() { Name = "Colegiul National Decebal Deva" };

        Assert.Throws<ClassroomNotRegisteredException>(() => repository.RemoveClassroom(classroom, school));
    }

    [Fact]
    public void GetLastId_NoSchools_ReturnsOne()
    {
        var repository = new SchoolRepository();

        var result = repository.GetLastId();

        Assert.Equal(1, result);
    }

    [Fact]
    public void GetLastId_ExistingSchools_ReturnsNextId()
    {
        var repository = new SchoolRepository();
        repository.Create(new School { ID = 1, Name = "Colegiul National Decebal Deva" });
        repository.Create(new School { ID = 3, Name = "Colegiul National Decebal Deva" });

        var result = repository.GetLastId();

        Assert.Equal(4, result);
    }

    [Fact]
    public void GetById_ExistingId_ReturnsSchool()
    {
        var repository = new SchoolRepository();
        var school = new School { ID = 1, Name = "Colegiul National Decebal Deva" };
        repository.Create(school);

        var result = repository.GetById(1);

        Assert.Equal(school, result);
    }

    [Fact]
    public void GetById_NonExistentId_ReturnsNull()
    {
        var repository = new SchoolRepository();

        var result = repository.GetById(999);

        Assert.Null(result);
    }

    [Fact]
    public void Create_AddsSchoolToList()
    {
        var school = new School() { Name = "Colegiul National Decebal Deva" };
        var classroomExceptionMock = new Mock<ClassroomException>();

        var repository = new SchoolRepository();

        var createdSchool = repository.Create(school);

        Assert.NotNull(createdSchool); 
    }

    [Fact]
    public void UpdateSchool_ExistingSchool_UpdatesSuccessfully()
    {
        // Arrange
        var repository = new SchoolRepository();
        var school = new School { Name = "Colegiul National Decebal Deva", ID = 1};
        repository.Create(school);

        var updatedSchool = new School { Name = "Updated School", ID = 2 };

        // Act
        repository.UpdateSchool(updatedSchool, school.ID);
        var retrievedSchool = repository.GetById(school.ID);

        // Assert
        Assert.Equal(updatedSchool.Name, retrievedSchool.Name);
        Assert.Equal(updatedSchool.Classrooms, retrievedSchool.Classrooms);
    }

    [Fact]
    public void UpdateSchool_NonExistingSchool_ThrowsException()
    {
        var repository = new SchoolRepository();
        var school = new School { ID = 1 , Name = "Colegiul National Decebal Deva" };

        Assert.Throws<TeacherNotFoundException>(() => repository.UpdateSchool(school, 1));
    }

    [Fact]
    public void Delete_RemovesSchoolFromListMethodCall()
    {

        var mockRepository = new Mock<ISchoolRepository>();
        var school = new School { ID = 1, Name = "Colegiul National Decebal Deva" };

 
        mockRepository.Setup(repo => repo.Create(It.IsAny<School>()));
        mockRepository.Setup(repo => repo.Delete(It.IsAny<School>()));


        mockRepository.Object.Create(school); 
        mockRepository.Object.Delete(school); 

        mockRepository.Verify(repo => repo.Delete(school), Times.Once);
    }

    [Fact]
    public void Delete_RemovesSchoolFromList()
    {
        var mockRepository = new Mock<ISchoolRepository>();
        var school = new School { ID = 1, Name = "Colegiul National Decebal Deva" };

        mockRepository.Setup(repo => repo.Create(It.IsAny<School>()));
        mockRepository.Setup(repo => repo.Delete(It.IsAny<School>()));

        mockRepository.Object.Create(school); 
        mockRepository.Object.Delete(school); 

        mockRepository.Verify(repo => repo.Delete(school), Times.Once);

        var schools = mockRepository.Object.GetAll();
        Assert.DoesNotContain(school, schools);
    }

    [Fact]
    public void Update_ExistingSchool_UpdatesSuccessfully()
    {
        var repository = new SchoolRepository();
        var school = new School { ID = 1 , Name = "Colegiul National Decebal Deva" };
        repository.Create(school);
        var updatedSchool = new School { ID = 1, Name = "Updated School" };

        var result = repository.Update(1, updatedSchool);

        Assert.Equal(updatedSchool, result);
    }

    [Fact]
    public void Update_NonExistingSchool_ThrowsException()
    {
        var repository = new SchoolRepository();
        var school = new School { ID = 1 , Name = "Colegiul National Decebal Deva" };

        Assert.Throws<StudentNotFoundException>(() => repository.Update(1, school));
    }

}