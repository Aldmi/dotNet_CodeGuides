using Domain.Persones;
using FluentAssertions;

namespace UnitTests.Persones;

public class PersoneNameTests
{
    //Правила именования тестов
    //[ThinqUnderTest]_Should_[ExpectedResult]_[Condition]

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Create_Should_ReturnNull_WhenValueIsNullOrEmpty(string? value)
    {
        //Act
        var obj = PersoneName.Create(value);

        //Assert
        obj.Should().BeNull();
    }
    
}