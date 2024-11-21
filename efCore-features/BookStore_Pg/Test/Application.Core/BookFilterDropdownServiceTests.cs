using Application.Core.BookFeatures.Query.ListBooksQuery.BookFilterDropdownQuery;
using Application.Core.BookFeatures.Query.ListBooksQuery.QueryObjects;
using Test.EfCoreDbContextTest;
using Test.TestHelpers;
using Xunit.Abstractions;
using Xunit.Extensions.AssertExtensions;

namespace Test.Application.Core;

[Collection("Database collection")]
public class BookFilterDropdownQueryTest
{
    private readonly DbContextFixture _fixture;
    private readonly ITestOutputHelper _output;

    public BookFilterDropdownQueryTest(DbContextFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        using var context = _fixture.DbContextFactory();
        context.ClearDb();
        context.SeedDatabaseFourBooks();
    }


    [Fact]
    public async Task GetFilterDropDownValues_Filetr_ByTags_Ok()
    {
        //arrange
        await using var context = _fixture.DbContextFactoryWithSqlLogs(_output.WriteLine);
        var service = new BookFilterDropdownQuery(context);

        //act
        var dropDownTuple= service.GetFilterDropDownValues(BooksFilterBy.ByTags).ToList();

        //assert
        dropDownTuple.Count.ShouldEqual(4);
        
        /*
           SELECT t."TagId" AS "Value"
           FROM "Tags" AS t
         */
    }
    
    
    [Fact]
    public async Task GetFilterDropDownValues_Filetr_ByVotes_Ok()
    {
        //arrange
        await using var context = _fixture.DbContextFactoryWithSqlLogs(_output.WriteLine);
        var service = new BookFilterDropdownQuery(context);

        //act
        var dropDownTuple= service.GetFilterDropDownValues(BooksFilterBy.ByVotes).ToList();

        //assert
        dropDownTuple.Count.ShouldEqual(4);
        
        /*
         */
    }
    
    
    [Fact]
    public async Task GetFilterDropDownValues_Filetr_ByPublicationYear_Ok()
    {
        //arrange
        await using var context = _fixture.DbContextFactoryWithSqlLogs(_output.WriteLine);
        var service = new BookFilterDropdownQuery(context);

        //act
        var dropDownTuple= service.GetFilterDropDownValues(BooksFilterBy.ByPublicationYear).ToList();

        //assert
        dropDownTuple.Count.ShouldEqual(4);
        
        /*
      SELECT t.c::text AS "Value"
      FROM (
          SELECT DISTINCT date_part('year', b."publishedOn")::int AS c
          FROM "Books" AS b
          WHERE b."publishedOn" <= @__today_0
      ) AS t
      ORDER BY t.c DESC
         */
    }
}