using Application.Core.BookFeatures.Query.ListBooksQuery;

namespace Application.Core.BookFeatures.Query.QueryObjects;


public enum OrderByOptions
{
    SimpleOrder,
    ByVotes,
    ByPublicationDate,
    ByPriceLowestFirst,
    ByPriceHighestFirst,
}

public static class BookListDtoSort
{
    public static IQueryable<BookListDto> OrderBooksBy(this IQueryable<BookListDto> books, OrderByOptions orderBy)
    {
        return orderBy switch
        {
            OrderByOptions.SimpleOrder => books.OrderByDescending(x => x.BookId),
            OrderByOptions.ByVotes => books.OrderByDescending(x => x.ReviewsAverageVotes),
            OrderByOptions.ByPublicationDate => books.OrderByDescending(x => x.PublishedOn),
            OrderByOptions.ByPriceLowestFirst => books.OrderBy(x => x.ActualPrice),
            OrderByOptions.ByPriceHighestFirst => books.OrderByDescending(x => x.ActualPrice),
            _ => throw new ArgumentOutOfRangeException(nameof(orderBy), orderBy, null)
        };
    }
}