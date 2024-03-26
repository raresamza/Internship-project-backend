namespace Student.Tests;

using Backend.Models;
using Student;

public class StudentTests
{

    private readonly Student _student;
    private readonly Student _student1;

    public StudentTests()
    {
        _student= new Student("mail@mail.com", "Adi", 11, 11111111, "Rares", "deva");
    }


    [Theory]
    [InlineData("Adi")]
    public void Constructor_ParentNameTest(string expectedParentName)
    {
        Assert.Equal(expectedParentName, _student.ParentName);
    }

    [Theory]
    [InlineData("mail@mail.com")]
    public void Constructor_ParentEmailTest(string expectedParentEmail)
    {
        Assert.Equal(expectedParentEmail, _student.ParentEmail);
    }

    [Theory]
    [InlineData(11)]
    public void Constructor_AgeTest(int expectedAge)
    {
        Assert.Equal(expectedAge, _student.Age);
    }

    [Theory]
    [InlineData(11111111)]
    public void Constructor_PhoneNumberTest(int expectedPhoneNumber)
    {

        Assert.Equal(expectedPhoneNumber, _student.PhoneNumber);
    }

    [Theory]
    [InlineData("Rares")]
    public void Constructor_NameTest(string expectedName)
    {
        Assert.Equal(expectedName, _student.Name);
    }

    [Theory]
    [InlineData("deva")]
    public void Constructor_AddressTest(string expectedAddress)
    {
        Assert.Equal(expectedAddress, _student.Address);
    }
}