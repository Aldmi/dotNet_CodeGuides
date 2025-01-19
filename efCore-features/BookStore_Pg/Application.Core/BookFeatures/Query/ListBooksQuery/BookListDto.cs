namespace Application.Core.BookFeatures.Query.ListBooksQuery;

public class BookListDto
{
    public long BookId { get; set; }
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