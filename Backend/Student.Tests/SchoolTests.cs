//namespace Student.Tests;

//using Backend.Domain.Models;
//using Backend.Exceptions.ClassroomException;
//using Backend.Exceptions.StudentException;
//using Backend.Exceptions.TeacherException;
//using Moq;
//using Backend.Application.Abstractions;

//public class SchoolTests
//{
//    [Fact]
//    public void SchoolAddClassroomAddsToClassroomsCollection()
//    {
//        // Arrange
//        var school = new School() { Name = "" };
//        var classroom = new Classroom() { Name = "" };

//        // Act
//        school.Classrooms.Add(classroom);

//        // Assert
//        Assert.Single(school.Classrooms);
//        Assert.Contains(classroom, school.Classrooms);
//    }

//    [Fact]
//    public void SchoolRemoveClassroomRemovesFromClassroomsCollection()
//    {
//        var school = new School() { Name = "" };
//        var classroom1 = new Classroom() { Name = "" };
//        var classroom2 = new Classroom() { Name = "" };
//        school.Classrooms.Add(classroom1);
//        school.Classrooms.Add(classroom2);

//        school.Classrooms.Remove(classroom1);

//        Assert.Multiple(() =>
//        {
//            Assert.Single(school.Classrooms);
//            Assert.DoesNotContain(classroom1, school.Classrooms);
//            Assert.Contains(classroom2, school.Classrooms);
//        });
//    }

//    [Fact]
//    public void SchoolConstructorWithNameSetsName()
//    {
//        string name = "Colegiul National Decebal Deva";

//        var school = new School() { Name = name };

//        Assert.Equal(name, school.Name);
//    }

//    [Fact]
//    public void SchoolDefaultConstructorSetsNameToEmptyString()
//    {
//        var school = new School() { Name = "" };

//        Assert.Equal("", school.Name);
//    }

//    [Fact]
//    public void SchoolDefaultConstructorSetsClassroomsToEmptyList()
//    {
//        var school = new School() { Name = "" };

//        Assert.Empty(school.Classrooms);
//    }

//    [Fact]
//    public void AddClassroomValidClassroomAddsToSchool()
//    {
//        var mockRepository = new Mock<ISchoolRepository>();
//        var classroom = new Classroom() { Name = "12B" };
//        var school = new School() { Name = "Colegiul National Decebal Deva" };

//        mockRepository.Setup(repo => repo.AddClassroom(classroom, school))
//                      .Callback((Classroom c, School s) => s.Classrooms.Add(c));

//        mockRepository.Object.AddClassroom(classroom, school);

//        Assert.Contains(classroom, school.Classrooms);
//    }

//    [Fact]
//    public void AddClassroomNullClassroomThrowsException()
//    {
//        var mockRepository = new Mock<ISchoolRepository>();
//        var school = new School() { Name = "Colegiul National Decebal Deva" };

//        mockRepository.Setup(repo => repo.AddClassroom(null, school))
//                      .Throws(new NullClassroomException("Classroom is not valid"));

//        // Act & Assert
//        Assert.Throws<NullClassroomException>(() => mockRepository.Object.AddClassroom(null, school));
//    }

//    [Fact]
//    public void AddClassroomAlreadyRegisteredClassroomThrowsException()
//    {
//        var mockRepository = new Mock<ISchoolRepository>();
//        var classroom = new Classroom() { Name = "12B" };
//        var school = new School() { Name = "Colegiul National Decebal Deva" };
//        school.Classrooms.Add(classroom);

//        mockRepository.Setup(repo => repo.AddClassroom(classroom, school))
//                     .Throws(new ClassroomAlreadyRegisteredException("Classroom is already registered"));


//        Assert.Throws<ClassroomAlreadyRegisteredException>(() => mockRepository.Object.AddClassroom(classroom, school));
//    }

//    [Fact]
//    public void RemoveClassroomValidClassroomRemovesFromClassrooms()
//    {
//        var mockRepository = new Mock<ISchoolRepository>();
//        var classroom = new Classroom() { Name = "12B" };
//        var school = new School() { Name = "Colegiul National Decebal Deva" };
//        school.Classrooms.Add(classroom);

//        mockRepository.Setup(repo => repo.RemoveClassroom(classroom, school))
//                      .Callback((Classroom c, School s) => s.Classrooms.Remove(c));

//        mockRepository.Object.RemoveClassroom(classroom, school);

//        Assert.DoesNotContain(classroom, school.Classrooms);
//    }

//    [Fact]
//    public void RemoveClassroomNullClassroomThrowsException()
//    {
//        var mockRepository = new Mock<ISchoolRepository>();
//        var school = new School() { Name = "Colegiul National Decebal Deva" };
//        mockRepository.Setup(repo => repo.RemoveClassroom(null, school))
//            .Throws(new NullClassroomException("Classroom not found"));

//        Assert.Throws<NullClassroomException>(() => mockRepository.Object.RemoveClassroom(null, school));
//    }

//    [Fact]
//    public void RemoveClassroomNotRegisteredClassroomThrowsException()
//    {
//        var mockRepository = new Mock<ISchoolRepository>();
//        var classroom = new Classroom() { Name = "12B" };
//        var school = new School() { Name = "Colegiul National Decebal Deva" };
//        mockRepository.Setup(repo => repo.RemoveClassroom(classroom, school))
//            .Throws(new ClassroomNotRegisteredException("Classroom is not registered"));

//        Assert.Throws<ClassroomNotRegisteredException>(() => mockRepository.Object.RemoveClassroom(classroom, school));
//    }

//    [Fact]
//    public void GetLastIdNoSchoolsReturnsOne()
//    {
//        var mockRepository = new Mock<ISchoolRepository>();
//        mockRepository.Setup(repo => repo.GetLastId()).Returns(1);

//        var result = mockRepository.Object.GetLastId();

//        Assert.Equal(1, result);
//    }

//    [Fact]
//    public void GetLastIdExistingSchoolsReturnsNextId()
//    {
//        var mockRepository = new Mock<ISchoolRepository>();

//        mockRepository.Setup(repo => repo.Create(It.IsAny<School>()))
//                      .Callback<School>(school =>
//                      {
//                          mockRepository.Setup(repo => repo.GetLastId()).Returns(school.ID);
//                      });

//        var result1 = mockRepository.Object.GetLastId();
//        mockRepository.Object.Create(new School { ID = 1, Name = "Colegiul National Decebal Deva" });
//        var result2 = mockRepository.Object.GetLastId();
//        mockRepository.Object.Create(new School { ID = 3, Name = "Colegiul National Decebal Deva" });
//        var result3 = mockRepository.Object.GetLastId();

//        Assert.Equal(0, result1);
//        Assert.Equal(1, result2);
//        Assert.Equal(3, result3);
//    }

//    [Fact]
//    public void GetByIdExistingIdReturnsSchool()
//    {
//        var mockRepository = new Mock<ISchoolRepository>();
//        var school = new School { ID = 1, Name = "Colegiul National Decebal Deva" };

//        mockRepository.Setup(repo => repo.Create(school));

//        mockRepository.Setup(repo => repo.GetById(1)).Returns(school);

//        mockRepository.Object.Create(school);
//        var result = mockRepository.Object.GetById(1);

//        Assert.Equal(school, result);
//    }

//    [Fact]
//    public void GetByIdNonExistentIdReturnsNull()
//    {
//        var mockRepository = new Mock<ISchoolRepository>();

//        mockRepository.Setup(repo => repo.GetById(999)).Returns((School)null);

//        var result = mockRepository.Object.GetById(999);

//        Assert.Null(result);
//    }

//    [Fact]
//    public void CreateAddsSchoolToList()
//    {
//        var school = new School { Name = "Colegiul National Decebal Deva" };

//        var repositoryMock = new Mock<ISchoolRepository>();

//        repositoryMock.Setup(repo => repo.Create(school)).Returns(school);


//        var createdSchool = repositoryMock.Object.Create(school);

//        Assert.NotNull(createdSchool);
//    }

//    [Fact]
//    public void UpdateSchoolExistingSchoolUpdatesSuccessfully()
//    {
//        var school = new School { Name = "Colegiul National Decebal Deva", ID = 1 };
//        var updatedSchool = new School { Name = "Updated School", ID = 1 };

//        var repositoryMock = new Mock<ISchoolRepository>();

//        repositoryMock.Setup(repo => repo.Update(school.ID, updatedSchool));

//        repositoryMock.Setup(repo => repo.GetById(school.ID)).Returns(updatedSchool);

//        repositoryMock.Object.Update(school.ID, updatedSchool);
//        var retrievedSchool = repositoryMock.Object.GetById(school.ID);

//        Assert.Equal(updatedSchool.Name, retrievedSchool.Name);
//        Assert.Equal(updatedSchool.Classrooms, retrievedSchool.Classrooms);
//    }

//    [Fact]
//    public void UpdateSchoolNonExistingSchoolThrowsException()
//    {
//        var repositoryMock = new Mock<ISchoolRepository>();
//        var school = new School { ID = 1, Name = "Colegiul National Decebal Deva" };
//        repositoryMock.Setup(repo => repo.Update(1, school))
//            .Throws(new TeacherNotFoundException("Teacher not found"));

//        Assert.Throws<TeacherNotFoundException>(() => repositoryMock.Object.Update(1, school));
//    }

//    [Fact]
//    public void DeleteRemovesSchoolFromListMethodCall()
//    {

//        var mockRepository = new Mock<ISchoolRepository>();
//        var school = new School { ID = 1, Name = "Colegiul National Decebal Deva" };


//        mockRepository.Setup(repo => repo.Create(It.IsAny<School>()));
//        mockRepository.Setup(repo => repo.Delete(It.IsAny<School>()));


//        mockRepository.Object.Create(school);
//        mockRepository.Object.Delete(school);

//        mockRepository.Verify(repo => repo.Delete(school), Times.Once);
//    }

//    [Fact]
//    public void DeleteRemovesSchoolFromList()
//    {
//        var mockRepository = new Mock<ISchoolRepository>();
//        var school = new School { ID = 1, Name = "Colegiul National Decebal Deva" };

//        mockRepository.Setup(repo => repo.Create(It.IsAny<School>()));
//        mockRepository.Setup(repo => repo.Delete(It.IsAny<School>()));

//        mockRepository.Object.Create(school);
//        mockRepository.Object.Delete(school);

//        mockRepository.Verify(repo => repo.Delete(school), Times.Once);

//        var schools = mockRepository.Object.GetAll();
//        Assert.DoesNotContain(school, schools);
//    }

//    [Fact]
//    public void UpdateExistingSchoolUpdatesSuccessfully()
//    {
//        var school = new School { ID = 1, Name = "Colegiul National Decebal Deva" };
//        var updatedSchool = new School { ID = 1, Name = "Updated School" };

//        var repositoryMock = new Mock<ISchoolRepository>();
//        repositoryMock.Setup(repo => repo.Create(school)).Returns(school);
//        repositoryMock.Setup(repo => repo.Update(1, updatedSchool)).Returns(updatedSchool);

//        var repository = repositoryMock.Object;
//        repository.Create(school);
//        var result = repository.Update(1, updatedSchool);

//        Assert.Equal(updatedSchool, result);
//    }

//    [Fact]
//    public void UpdateNonExistingSchoolThrowsException()
//    {
//        var repositoryMock = new Mock<ISchoolRepository>();

//        var school = new School { ID = 1, Name = "Colegiul National Decebal Deva" };
//        repositoryMock.Setup(repo => repo.Update(1,school))
//            .Throws(new StudentNotFoundException("Student not found"));
//        Assert.Throws<StudentNotFoundException>(() => repositoryMock.Object.Update(1, school));
//    }
//}