using Microsoft.EntityFrameworkCore;
using Test.TestHelpers;
using Xunit.Abstractions;

namespace Test.EfCoreDbContextTest;

[Collection("Database collection")]
public class GetOneItemTest
{
    private readonly DbContextFixture _fixture;
    private readonly ITestOutputHelper _output;

    public GetOneItemTest(DbContextFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        using var context = _fixture.DbContextFactory();
        context.ClearDb();
        context.SeedDatabaseFourBooks();
    }


    [Fact]
    public async Task First_Book_Ok()
    {
        //arrange
        await using var context = _fixture.DbContextFactoryWithSqlLogs(_output.WriteLine);
        
        var book= await context.Books.FirstOrDefaultAsync(b=>b.Title=="Refactoring"); 
        
        /*
        SELECT b."bookId", b."Description", b."ImageUrl", b."Price", b."publishedOn", b."Publisher", b."SoftDeleted", b."Title"
        FROM "Books" AS b
        WHERE b."Title" = 'Refactoring'
        LIMIT 1
        */
    }
    
    [Fact]
    public async Task Single_Book_Ok()
    {
        //arrange
        await using var context = _fixture.DbContextFactoryWithSqlLogs(_output.WriteLine);
        
        var book= await context.Books.SingleOrDefaultAsync(b=>b.Title=="Refactoring");
        
        /*
        SELECT b."bookId", b."Description", b."ImageUrl", b."Price", b."publishedOn", b."Publisher", b."SoftDeleted", b."Title"
        FROM "Books" AS b
        WHERE b."Title" = 'Refactoring'
        LIMIT 2
        */
    }
    
    
    [Fact]
    public async Task Last_Book_Ok()
    {
        //arrange
        await using var context = _fixture.DbContextFactoryWithSqlLogs(_output.WriteLine);
        
        //LastOrDefaultAsync - используется только с сортировкой
        var book= await context.Books
            .OrderBy(b=>b.Title)
            .LastOrDefaultAsync(b=>EF.Functions.Like(b.Title, "%fac%"));
        
        /*
          SELECT b."bookId", b."Description", b."ImageUrl", b."Price", b."publishedOn", b."Publisher", b."SoftDeleted", b."Title"
          FROM "Books" AS b
          WHERE b."Title" LIKE '%fac%'
          ORDER BY b."Title" DESC
          LIMIT 1
        */
    }
    
    
    [Fact]
    public async Task Find_Book_Ok()
    {
        //arrange
        await using var context = _fixture.DbContextFactoryWithSqlLogs(_output.WriteLine);
        
        var book= await context.Books.FindAsync(1);
        
        /*
      SELECT b."bookId", b."Description", b."ImageUrl", b."Price", b."publishedOn", b."Publisher", b."SoftDeleted", b."Title"
      FROM "Books" AS b
      WHERE b."bookId" = @__p_0
      LIMIT 1
        */
    }
}