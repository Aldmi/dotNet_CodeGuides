using Application.Core.BookFeatures.Query.QueryObjects;
using Test.EfCoreDbContextTest;
using Test.TestHelpers;
using Xunit.Abstractions;
using Xunit.Extensions.AssertExtensions;

namespace Test.Application.Core;

[Collection("Database collection")]
public class BookServiceQueryObjectTests
{
    private readonly DbContextFixture _fixture;
    private readonly ITestOutputHelper _output;

    public BookServiceQueryObjectTests(DbContextFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        using var context = _fixture.DbContextFactory();
        context.ClearDb();
        context.SeedDatabaseFourBooks();
    }
    
    
    /// <summary>
    /// ВЫБОРОЧНАЯ ЗАГРУЗКА (использование LINQ Select).
    /// Для загрузки связанных данных не нужно использовать "Немедленную загрузку", те Inclue().
    /// </summary>
    [Fact]
    public async Task BookListDtoSelect_Ok()
    {
        //arrange
        await using var context = _fixture.DbContextFactoryWithSqlLogs(_output.WriteLine);

        //act
        var books = context.Books.MapBookToDto().ToList();
        
        //assert
        books.Count.ShouldEqual(4);
        
        /*
      SELECT b."bookId", b."Title", b."Price", b."publishedOn", CASE
          WHEN p."PriceOfferId" IS NULL THEN b."Price"
          ELSE p."NewPrice"
      END, p."PromotionalText", p."PriceOfferId", t."Name", t."BookId", t."AuthorId", t."authorId0", (
          SELECT count(*)::int
          FROM "Review" AS r
          WHERE b."bookId" = r."BookId"), (
          SELECT avg(r0."NumStars"::double precision)
          FROM "Review" AS r0
          WHERE b."bookId" = r0."BookId"), t0."TagId", t0.book_id, t0.tag_id
      FROM "Books" AS b
      LEFT JOIN "PriceOffers" AS p ON b."bookId" = p."BookId"
      LEFT JOIN (
          SELECT a."Name", b0."BookId", b0."AuthorId", a."authorId" AS "authorId0", b0."order"
          FROM "BookAuthor" AS b0
          INNER JOIN "Authors" AS a ON b0."AuthorId" = a."authorId"
      ) AS t ON b."bookId" = t."BookId"
      LEFT JOIN (
          SELECT t1."TagId", b1.book_id, b1.tag_id
          FROM "BookTag" AS b1
          INNER JOIN "Tags" AS t1 ON b1.tag_id = t1."TagId"
      ) AS t0 ON b."bookId" = t0.book_id
      ORDER BY b."bookId", p."PriceOfferId", t."order", t."BookId", t."AuthorId", t."authorId0", t0.book_id, t0.tag_id
         */
    }
    
    
    [Fact]
    public async Task BookListDtoSort_Ok()
    {
        //arrange
        await using var context = _fixture.DbContextFactoryWithSqlLogs(_output.WriteLine);
        var query = context.Books.MapBookToDto();
       

        //act
        var books = query.OrderBooksBy(OrderByOptions.ByVotes).ToList();
        
        //assert
        books.Count.ShouldEqual(4);
        
        
        /*
      SELECT b."bookId", b."Title", b."Price", b."publishedOn", CASE
          WHEN p."PriceOfferId" IS NULL THEN b."Price"
          ELSE p."NewPrice"
      END, p."PromotionalText", p."PriceOfferId", t."Name", t."BookId", t."AuthorId", t."authorId0", (
          SELECT count(*)::int
          FROM "Review" AS r0
          WHERE b."bookId" = r0."BookId"), (
          SELECT avg(r1."NumStars"::double precision)
          FROM "Review" AS r1
          WHERE b."bookId" = r1."BookId"), t0."TagId", t0.book_id, t0.tag_id
      FROM "Books" AS b
      LEFT JOIN "PriceOffers" AS p ON b."bookId" = p."BookId"
      LEFT JOIN (
          SELECT a."Name", b0."BookId", b0."AuthorId", a."authorId" AS "authorId0", b0."order"
          FROM "BookAuthor" AS b0
          INNER JOIN "Authors" AS a ON b0."AuthorId" = a."authorId"
      ) AS t ON b."bookId" = t."BookId"
      LEFT JOIN (
          SELECT t1."TagId", b1.book_id, b1.tag_id
          FROM "BookTag" AS b1
          INNER JOIN "Tags" AS t1 ON b1.tag_id = t1."TagId"
      ) AS t0 ON b."bookId" = t0.book_id
      ORDER BY (
          SELECT avg(r."NumStars"::double precision)
          FROM "Review" AS r
          WHERE b."bookId" = r."BookId") DESC, b."bookId", p."PriceOfferId", t."order", t."BookId", t."AuthorId", t."authorId0", t0.book_id, t0.tag_id
         */
    }
    
    
    [Fact]
    public async Task BookListDtoFilter_ByVotes_Ok()
    {
        //arrange
        await using var context = _fixture.DbContextFactoryWithSqlLogs(_output.WriteLine);
        var query = context.Books.MapBookToDto();
       

        //act
        var books = query.FilterBooksBy(BooksFilterBy.ByVotes, "2").ToList();
        
        //assert
        books.Count.ShouldEqual(1);
        
        
        /*
      SELECT b."bookId", b."Title", b."Price", b."publishedOn", CASE
          WHEN p."PriceOfferId" IS NULL THEN b."Price"
          ELSE p."NewPrice"
      END, p."PromotionalText", p."PriceOfferId", t."Name", t."BookId", t."AuthorId", t."authorId0", (
          SELECT count(*)::int
          FROM "Review" AS r0
          WHERE b."bookId" = r0."BookId"), (
          SELECT avg(r1."NumStars"::double precision)
          FROM "Review" AS r1
          WHERE b."bookId" = r1."BookId"), t0."TagId", t0.book_id, t0.tag_id
      FROM "Books" AS b
      LEFT JOIN "PriceOffers" AS p ON b."bookId" = p."BookId"
      LEFT JOIN (
          SELECT a."Name", b0."BookId", b0."AuthorId", a."authorId" AS "authorId0", b0."order"
          FROM "BookAuthor" AS b0
          INNER JOIN "Authors" AS a ON b0."AuthorId" = a."authorId"
      ) AS t ON b."bookId" = t."BookId"
      LEFT JOIN (
          SELECT t1."TagId", b1.book_id, b1.tag_id
          FROM "BookTag" AS b1
          INNER JOIN "Tags" AS t1 ON b1.tag_id = t1."TagId"
      ) AS t0 ON b."bookId" = t0.book_id
      WHERE (
          SELECT avg(r."NumStars"::double precision)
          FROM "Review" AS r
          WHERE b."bookId" = r."BookId") > @__p_0
      ORDER BY b."bookId", p."PriceOfferId", t."order", t."BookId", t."AuthorId", t."authorId0", t0.book_id, t0.tag_id
         */
    }
    
    
    [Fact]
    public async Task BookListDtoFilter_ByPublicationYear_Ok()
    {
        //arrange
        await using var context = _fixture.DbContextFactoryWithSqlLogs(_output.WriteLine);
        var query = context.Books.MapBookToDto();
       

        //act
        var books = query.FilterBooksBy(BooksFilterBy.ByPublicationYear, "1999").ToList();
        
        //assert
        books.Count.ShouldEqual(1);
        
        
        /*
      SELECT b."bookId", b."Title", b."Price", b."publishedOn", CASE
          WHEN p."PriceOfferId" IS NULL THEN b."Price"
          ELSE p."NewPrice"
      END, p."PromotionalText", p."PriceOfferId", t."Name", t."BookId", t."AuthorId", t."authorId0", (
          SELECT count(*)::int
          FROM "Review" AS r
          WHERE b."bookId" = r."BookId"), (
          SELECT avg(r0."NumStars"::double precision)
          FROM "Review" AS r0
          WHERE b."bookId" = r0."BookId"), t0."TagId", t0.book_id, t0.tag_id
      FROM "Books" AS b
      LEFT JOIN "PriceOffers" AS p ON b."bookId" = p."BookId"
      LEFT JOIN (
          SELECT a."Name", b0."BookId", b0."AuthorId", a."authorId" AS "authorId0", b0."order"
          FROM "BookAuthor" AS b0
          INNER JOIN "Authors" AS a ON b0."AuthorId" = a."authorId"
      ) AS t ON b."bookId" = t."BookId"
      LEFT JOIN (
          SELECT t1."TagId", b1.book_id, b1.tag_id
          FROM "BookTag" AS b1
          INNER JOIN "Tags" AS t1 ON b1.tag_id = t1."TagId"
      ) AS t0 ON b."bookId" = t0.book_id
      WHERE date_part('year', b."publishedOn")::int = @__filterYear_0 AND b."publishedOn" <= DATE '2024-11-20'
      ORDER BY b."bookId", p."PriceOfferId", t."order", t."BookId", t."AuthorId", t."authorId0", t0.book_id, t0.tag_id
         */
    }
}
