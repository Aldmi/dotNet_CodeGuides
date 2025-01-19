
namespace Domain.Books.Entities
{
    public class Review //#L
    {
        public long ReviewId { get; set; }
        public string VoterName { get; set; }
        public int NumStars { get; set; }
        public string? Comment { get; set; }

        //-----------------------------------------
        //Relationships

        public long BookId { get; set; } //#M
    }

    /*******************************************************
    #L This holds customer reviews with their rating
    #M This foreign key holds the key of the book this review belongs to
     * *****************************************************/
}