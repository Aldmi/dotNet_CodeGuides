using Domain.Books.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Test.TestHelpers;
using Xunit.Abstractions;
using Xunit.Extensions.AssertExtensions;

namespace Test.EfCoreDbContextTest.Write;

[Collection("Database collection")]
public class UpdateBookTest 
{
    private readonly DbContextFixture _fixture;
    private readonly ITestOutputHelper _output;

    public UpdateBookTest(DbContextFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        using var context = _fixture.DbContextFactory();
        context.ClearDb();
        context.SeedDatabaseFourBooks();
    }
    
    
    [Fact]
    public async Task Update_With_Tracking_Book_Ok()
    {
        //arrange
        await using var context = _fixture.DbContextFactory();
        var book = await context.Books.FirstAsync(b => b.Title == "Refactoring");
        
        //act
        book.Title = "New Title";
        var n= await context.SaveChangesAsync();
        
        //assert
        n.Should().Be(1);
    }
    
    
    
    [Fact]
    public async Task Update_With_Tracking_Book_Remove_Tags()
    {
        //Первоночальная книга со всеми связями
        List<Tag> originalTagList;
        Book originalBook;
        await using (var context = _fixture.DbContextFactory())
        {
            originalBook = await context.Books
                .Include(b => b.AuthorsLink)
                .ThenInclude(ba => ba.Author)
                .Include(b => b.Reviews)
                .Include(b => b.Tags)
                .SingleAsync(b => b.Title == "Refactoring");

            originalTagList = originalBook.Tags.ToList();
            //удалил все теги из книги
            originalBook.Tags = null;
                
            var n= await context.SaveChangesAsync();
            //n.Should().Be(1);
        }
        
        
        //Читаем заново со всеми полями
        Book finishedBook;
        await using (var context = _fixture.DbContextFactory())
        {
            finishedBook=await context.Books
                .Include(b=>b.AuthorsLink)
                .ThenInclude(ba=>ba.Author)
                .Include(b=>b.Reviews)
                .Include(b=>b.Tags)
                .SingleAsync(b => b.Id == originalBook.Id);
        }

        originalTagList.Count.Should().Be(2);
        finishedBook.Tags.ShouldBeEmpty();
    }


    [Fact]
    public async Task Update_Book_WithOut_Tracking_Book_Ok()
    {
        string json;
        long bookId;
        await using (var context = _fixture.DbContextFactory())
        {
            var book=await context.Books.SingleAsync(b => b.Title == "Refactoring");
            book.Title = "New Title";
            book.Description = "New Description";
            json = JsonConvert.SerializeObject(book);
            bookId = book.Id;
        }
        
        await using (var context = _fixture.DbContextFactory())
        {
            var book = JsonConvert.DeserializeObject<Book>(json);
            context.Books.Update(book);
            var entries = context.ChangeTracker.Entries().ToList();
            var n= await context.SaveChangesAsync();
            //assert
            n.Should().Be(1);
        }
        
        //Читаем заново со всеми полями
        await using (var context = _fixture.DbContextFactory())
        {
            var finishedBook= await context.Books
                .Include(b=>b.AuthorsLink)
                .ThenInclude(ba=>ba.Author)
                .Include(b=>b.Reviews)
                .Include(b=>b.Tags)
                .SingleAsync(b => b.Id == bookId);
        }
    }
    
    
    
    /// <summary>
    /// Отключенное обновление.
    /// 1ый запрос загружает книгу с авторами и отзыввами
    /// 2ой запрос получает измененную книгу, имя автора поменяли, и добавили отзыв - эти изменения сохраняются в БД.
    /// Update - сущности где есть Id проставит Modified, где нет Id Added.
    /// Поэтому для метода Update нужно передавать только необходимые связи. (например связующая таблица BookAuthor тоже будет обновлена, хотя меняли только имя автора)
    /// </summary>
    [Fact]
    public async Task Update_WithOut_Tracking_Book_With_Related_Data_OK()
    {
        string json;
        await using (var context = _fixture.DbContextFactory())
        {
            var book=await context.Books
                //.Include(b=>b.AuthorsLink)
                //.ThenInclude(ba=>ba.Author)
                .Include(b=>b.Reviews)
                .SingleAsync(b => b.Title == "Refactoring");
            
            //поменяли поля в книге
            book.Title = "New Title";
            book.Description = "New Description";
            
            //поменяли имя автора
            //book.AuthorsLink.First().Author.Name = "New Author";
            
            //добавили новый коментарий
            book.Reviews.Add(new Review{Comment = "New Comment", NumStars = 4, VoterName = "Alex"});
            
            var entries = context.ChangeTracker.Entries().ToList(); //4 изменения
            json = JsonConvert.SerializeObject(
                book,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore //не будет сериализовать объект, если он является дочерним объектом самого себя.
                });
        }
        
        await using (var context = _fixture.DbContextFactory())
        {
            var book = JsonConvert.DeserializeObject<Book>(json);
            context.Books.Update(book); 
            var entries = context.ChangeTracker.Entries().ToList(); //4 изменения
            var n= await context.SaveChangesAsync();
            //assert
            //n.Should().Be(4);
        }
        
        //asssert
        await using (var context = _fixture.DbContextFactory())
        {
            var book=await context.Books
                .Include(b=>b.AuthorsLink)
                .ThenInclude(ba=>ba.Author)
                .Include(b=>b.Reviews)
                .SingleAsync(b => b.Title == "New Title");

            book.Description.Should().Be("New Description");
            // book.AuthorsLink.First().Author.Name.Should().Be("New Author");
             book.Reviews.First().Comment.Should().Be("New Comment");
        }
    }
    
    
        
    [Fact]
    public async Task Update_Bulk_Book_Ok()
    {
        //arrange
        await using var context = _fixture.DbContextFactory();
        
        //act
        var rowsUpdated = await context.Books
            .Where(b => b.Title == "Refactoring")
            .ExecuteUpdateAsync(calls =>
                calls
                    .SetProperty(b => b.Title, b => b.Title + "New Title")
                    .SetProperty(b =>b.Price, b=> 999));
        
        //Вызывать context.SaveChanges() не нужно!!!
        
        //assert
        rowsUpdated.Should().Be(1);// изменили одну строку
    }
    
    
    
    /// <summary>
    /// вызов метода Update на объекте без связей не удаляет связи из БД. (если объект не отслеживается)
    /// </summary>
    [Fact]
    public async Task Update_WithOut_Tracking_Book_With_Related_Data_NOT_Rewrite_Related_Null()
    {
        //Первоночальная книга со всеми связями
        Book originalBook;
        await using (var context = _fixture.DbContextFactory())
        {
            originalBook=await context.Books
                .Include(b=>b.AuthorsLink)
                .ThenInclude(ba=>ba.Author)
                
                .Include(b=>b.Reviews)
                .Include(b=>b.Tags)
              
                .SingleAsync(b => b.Title == "Refactoring");
        }
        
        //Обновляем только ЧАСТЬ сущности (Tags = null, AuthorsLink = null)
        //поле Title не обновляем, но после вызова Update оно тоже будет помеченно Modified и переписанно
        await using (var context = _fixture.DbContextFactory())
        {
            var book = Book.Create(originalBook.Id);
            book.Title = originalBook.Title;
            book.Description = "New Description";
            book.Price = 100;
            // var book = new Book
            // {
            //     Id = originalBook.Id,
            //     Title = originalBook.Title,
            //     Description = "New Description",
            //     Price = 100,
            // };
            
            context.Books.Update(book);
            var n= await context.SaveChangesAsync();
            //assert
            n.Should().Be(1);
        }
        
        //Читаем заново со всеми полями
        Book finishedBook;
        await using (var context = _fixture.DbContextFactory())
        {
            finishedBook=await context.Books
                .Include(b=>b.AuthorsLink)
                .ThenInclude(ba=>ba.Author)
                
                .Include(b=>b.Reviews)
                .Include(b=>b.Tags)
              
                .SingleAsync(b => b.Id == originalBook.Id);
        }
        
        //ПОЛЯ СВЯЗЕЙ НЕ ПЕРЕПИСАЛИСЬ НА NULL. Обновились только указанные свойства.
        finishedBook.Tags.Count.Should().Be(originalBook.Tags.Count);
        finishedBook.AuthorsLink.Count.Should().Be(originalBook.AuthorsLink.Count);
        finishedBook.Description.Should().Be("New Description");
        finishedBook.Price.Should().Be(100);
    }



    [Fact]
    public async Task Update_M2M_BookAuthor_Explicity()
    {
        //Запрос на получения bookId и authorId, к книге добавить нового автора.
        long bookId;
        long authorId;
        await using (var context = _fixture.DbContextFactoryWithSqlLogs(_output.WriteLine))
        {
            var book= await context.Books.SingleAsync(b => b.Title == "Refactoring");
            bookId = book.Id;
            var author= await context.Authors.SingleAsync(a => a.Name == "Eric Evans");
            authorId = author.AuthorId;
        }
        
        //Пишем сразу в таблицу BookAuthor bookId и authorId Полученные из запроса
        await using (var context = _fixture.DbContextFactoryWithSqlLogs(_output.WriteLine))
        {
            var bookAuthor = await context.Set<BookAuthor>().AddAsync(new BookAuthor
            {
                BookId = (int)bookId,
                AuthorId = authorId,
                Order = 2
            });
            
            var n= await context.SaveChangesAsync(); //Добавили новую строку в БД
            n.Should().Be(1);
        }
    }
}