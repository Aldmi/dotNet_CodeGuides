
using Domain.Books.DomainEvents;
using Domain.Books.Primitives;

namespace Domain.Books.Entities
{
    public class Book : AgregateRoot //#A
    {
        //public long BookId { get; set; } //#B
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly PublishedOn { get; set; }
        public string? Publisher { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }

        public bool SoftDeleted { get; set; }

        //-----------------------------------------------
        //relationships
        public PriceOffer? Promotion { get; set; } //#C
        public ICollection<Review>? Reviews { get; set; } //#D
        public ICollection<Tag> Tags { get; set; } //#E
        public ICollection<BookAuthor> AuthorsLink { get; set; } //#F


        public void BuisnesLogicInDomainModel()
        {
            RaiseDomainEvent(new CreateBookDomainEvent(Id, Title));
        }

        public static Book Create(long id)
        {
            return new Book{Id = id};
        }
    }

    /****************************************************#
    #A The Book class contains the main book information
    #B I use EF Core's 'by convention' approach to defining the primary key of this entity class. In this case I use <ClassName>Id, and because the property if of type int EF Core assumes that the database will use the SQL IDENTITY command to create a unique key when a new row is added
    #C This is the link to the optional PriceOffer
    #D There can be zero to many Reviews of the book
    #E This is an EF Core 5's automatic many-to-many relationship to the Tag entity class
    #F This provides a link to the Many-to-Many linking table that links to the Authors of this book
     * **************************************************/
}