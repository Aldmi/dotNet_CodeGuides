using Domain.Books.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.TestHelpers;
using Xunit.Abstractions;
using Xunit.Extensions.AssertExtensions;

namespace Test.EfCoreDbContextTest.Write;

[Collection("Database collection")]
public class CreateBookTests
{
    private readonly DbContextFixture _fixture;
    public CreateBookTests(DbContextFixture fixture)
    {
        _fixture = fixture;
        using var context = _fixture.DbContextFactory();
        context.ClearDb();
        context.SeedDatabaseFourBooks();
    }
    
    
    [Fact]
    public async Task AddNewBook_With_New_Author_Ok()
    {       
        //arrange
        await using var context = _fixture.DbContextFactory();
        var book = new Book
        {
            Title = "test",
            Description = "test book",
            PublishedOn = new DateOnly(2003, 8, 30),
            Price = 666,
            //Tags = new List<Tag> { architectureTag, editorsChoice }
            AuthorsLink = new List<BookAuthor> {new() {Author = new Author {Name = "New Test Author"}}}
        };
        
        //act
        var entry = await context.AddAsync(book);
        var n= await context.SaveChangesAsync();

        //assert
        n.ShouldEqual(3);// записали в 3 таблицы
        entry.Entity.Title.Should().Be(book.Title);
        entry.Entity.Id.Should().BeGreaterThan(0);
    }
    
    
    [Fact]
    public async Task AddNewBook_With_Existing_Author_Ok()
    {
        //arrange
        await using var context = _fixture.DbContextFactory();
        var author = await context.Authors.FirstAsync(author1 => author1.Name == "Martin Fowler");
        var book = new Book
        {
            Title = "test",
            Description = "test book",
            PublishedOn = new DateOnly(2003, 8, 30),
            Price = 666,
            //Tags = new List<Tag> { architectureTag, editorsChoice }
            AuthorsLink = new List<BookAuthor>()
        };
        book.AuthorsLink.Add(new BookAuthor{Author = author, Order = 10});
        
        //act
        var entry = await context.AddAsync(book);
        var n= await context.SaveChangesAsync();

        //assert
        n.Should().Be(2);// записали в 2 таблицы
        entry.Entity.Title.Should().Be(book.Title);
        entry.Entity.Id.Should().BeGreaterThan(0);
    }
}