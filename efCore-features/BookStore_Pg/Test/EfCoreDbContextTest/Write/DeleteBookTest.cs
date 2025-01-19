using Domain.Books.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Test.TestHelpers;
using Xunit.Abstractions;

namespace Test.EfCoreDbContextTest.Write;

[Collection("Database collection")]
public class DeleteBookTest
{
    private readonly DbContextFixture _fixture;
    private readonly ITestOutputHelper _output;

    public DeleteBookTest(DbContextFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        using var context = _fixture.DbContextFactory();
        context.ClearDb();
        context.SeedDatabaseFourBooks();
    }


    [Fact]
    public async Task Delete_Tracking_CascadeDelete_Ok()
    {
        //arrange
        Book currentBook;
        await using (var context =  _fixture.DbContextFactoryWithSqlLogs(_output.WriteLine))
        {
            currentBook = await context.Books
                .Include(b=> b.Promotion)
                .Include(b=> b.Reviews)
                .Include(b=> b.Tags)
                .Include(b=> b.AuthorsLink)
                .SingleAsync(b => b.Title == "Quantum Networking");
        
            //act
            //Удаление из всех зависмых таблиц силами самого EF_core. тк он знает про все связи.
            context.Books.Remove(currentBook);
            var entries = context.ChangeTracker.Entries().ToList(); //DEBUG
            var n= await context.SaveChangesAsync();
        }

        
        //assert
        //состояние после удаления
        await using (var context = _fixture.DbContextFactory())
        {
            var book = await context.Books.SingleOrDefaultAsync(b => b.Id == currentBook.Id);
            var ba = await context.Set<BookAuthor>().Where(ba => ba.BookId == currentBook.Id).ToListAsync();
            var priceOffer =await context.PriceOffers.SingleOrDefaultAsync(po => po.PriceOfferId == currentBook.Promotion.PriceOfferId);
            var rewies = await context.Set<Review>().Where(review => review.BookId == currentBook.Id).ToListAsync();

            book.Should().BeNull();
            ba.Count.Should().Be(0);
            priceOffer.Should().BeNull();
            rewies.Count.Should().Be(0);
        }
    }
    
    
    [Fact]
    public async Task Delete_WithOut_Tracking_CascadeDelete_Ok()
    {
        //arange
        //Состояние ДО удаления
        Book currentBook;
        await using (var context = _fixture.DbContextFactory())
        {
            currentBook = await context.Books
                .Include(b=> b.Promotion)
                .Include(b=> b.Reviews)
                .Include(b=> b.Tags)
                .Include(b=> b.AuthorsLink)
                .SingleAsync(b => b.Title == "Quantum Networking");

            currentBook.Should().NotBeNull();
            currentBook.AuthorsLink.Count.Should().Be(2);
            currentBook.Tags.Count.Should().Be(1);
            currentBook.Promotion.NewPrice.Should().Be(219);
            currentBook.Reviews.Count.Should().Be(2);
        }
        

        //act
        //удаление книги, СУБД берет на себя каскадное удаление зависимых сущностей
        await using (var context = _fixture.DbContextFactoryWithSqlLogs(_output.WriteLine))
        {
            var book = await context.Books.SingleAsync(b => b.Title == "Quantum Networking");
            context.Books.Remove(book);
            var n= await context.SaveChangesAsync();
            n.Should().Be(1);
        }
        
        
        //assert
        //состояние после удаления
        await using (var context = _fixture.DbContextFactory())
        {
            var book = await context.Books.SingleOrDefaultAsync(b => b.Id == currentBook.Id);
            var ba = await context.Set<BookAuthor>().Where(ba => ba.BookId == currentBook.Id).ToListAsync();
            var priceOffer =await context.PriceOffers.SingleOrDefaultAsync(po => po.PriceOfferId == currentBook.Promotion.PriceOfferId);

            book.Should().BeNull();
            ba.Count.Should().Be(0);
            priceOffer.Should().BeNull();
        }
    }
}