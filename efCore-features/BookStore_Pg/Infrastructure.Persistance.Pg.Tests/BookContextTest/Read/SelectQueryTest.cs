using Infrastructure.Persistance.Pg.Tests.EfCoreDbContextTest;
using Infrastructure.Persistance.Pg.Tests.TestHelpers;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using Xunit.Extensions.AssertExtensions;

namespace Infrastructure.Persistance.Pg.Tests.BookContextTest.Read;

[Collection("Database collection")]
public class SelectQueryTest
{
    private readonly DbContextFixture _fixture;
    private readonly ITestOutputHelper _output;

    public SelectQueryTest(DbContextFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        using var context = _fixture.DbContextFactory();
        context.ClearDb();
        context.SeedDatabaseFourBooks();
    }
    
    
    [Fact]
    public async Task SelectToDto_Ok()
    {
        //arrange
        await using var context = _fixture.DbContextFactoryWithSqlLogs(_output.WriteLine);

        var books = context.Books.Select(b => new BookListDto()
        {
            BookId = b.BookId,
            Title = b.Title,
            Price = b.Price,
            PublishedOn = b.PublishedOn,
            ActualPrice = b.Promotion == null ? b.Price : b.Promotion.NewPrice,
            PromotinalText = b.Promotion == null ? null : b.Promotion.PromotionalText,
            AuthorsOrdered = string.Join(", ",
                b.AuthorsLink
                    .OrderBy(ba=>ba.Order)
                    .Select(ba=>ba.Author.Name)),
            ReviewsCount = b.Reviews.Count,
            ReviewsAverageVotes = b.Reviews.Select(r=>(double?)r.NumStars).Average(),
            TagStrings = b.Tags.Select(t=>t.TagId).ToArray()
        }).ToList();
        
        //assert
        books.Count.ShouldEqual(4);
    }

    
    internal class BookListDto
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public DateOnly PublishedOn { get; set; }
        public decimal Price { get; set; }
        public decimal ActualPrice { get; set; }
        public string? PromotinalText { get; set; }
        public string AuthorsOrdered { get; set; }
        public int ReviewsCount { get; set; }
        public double? ReviewsAverageVotes { get; set; }
        public string[] TagStrings { get; set; }
    }
}