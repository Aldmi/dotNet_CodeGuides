using Domain.Persones;
using FluentAssertions;

namespace UnitTests.Persones;


/// <summary>
/// ClassData - хорошо подходит для написания параметризированных со строго типизированными параметрами.
/// </summary>
public class PersoneTest
{
    [Theory]
    [ClassData(typeof(PersoneCreateTestData))]
    public void Create_Should_ReturnNull_WhenParamInvalid(string? inn, string? email, int age)
    {
        //arrange
        var personeName = PersoneName.Create("Al");
        var persone = Persone.Create(personeName, inn, email, age);
        
        
        
        //Assert
        persone.Should().BeNull();
    }
}


public class PersoneCreateTestData : TheoryData<string?, string?, int>
{
    public PersoneCreateTestData()
    {
        Add("", "Al@mail.ru", 52);
        Add("Al", "", 10);
        Add("", "Al@mail.ru", 10);
    }
}