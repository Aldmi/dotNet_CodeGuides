using Application.Core.Abstract;
using Domain.Books.Entities;

namespace Application.Core.BookFeatures.Command.CreateBook;

public class CreateBookService
{
    private readonly IBookContext _bookContext;

    public CreateBookService(IBookContext bookContext)
    {
        _bookContext = bookContext;
    }


    public async Task CreateTestBook()
    {
        var editorsChoice = new Tag { TagId = "Editor's Choice" };
        var refactoring = new Tag {TagId = "Refactoring"};
        
        var martinFowler = new Author
        {
            Name = "Martin Fowler"
        };
        
        var book1 = new Book
        {
            Title = "Refactoring",
            Description = "Improving the design of existing code",
            PublishedOn = new DateOnly(1999, 7, 8),  //new DateTime(1999, 7, 8).ToUniversalTime(),
            Price = 40,
            Tags = new List<Tag> { refactoring, editorsChoice }
        };
        book1.AuthorsLink = new List<BookAuthor> {new() {Author = martinFowler, Book = book1}};
        
        book1.BuisnesLogicInDomainModel(); //В реальном примере это был бы стратический метод Book.Create() 
        
        _bookContext.Books.Add(book1);
        await _bookContext.SaveChangesAsync();
    }
}