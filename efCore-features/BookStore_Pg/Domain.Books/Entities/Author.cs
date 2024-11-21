
using CSharpFunctionalExtensions;

namespace Domain.Books.Entities
{
    public class Author//#E
    {
        public long AuthorId { get; set; }
        public string Name { get; set; }

        //------------------------------
        //Relationships

        public ICollection<BookAuthor> BooksLink { get; set; } //#F
    }

    /*********************************************************
    #E The Author class just contains the name of the author
    #F This points to, via the linking table, all the books the Author has participated in
     * *****************************************************/
}