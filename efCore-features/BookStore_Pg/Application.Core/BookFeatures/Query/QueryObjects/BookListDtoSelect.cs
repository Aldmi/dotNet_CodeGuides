using Application.Core.BookFeatures.Query.ListBooksQuery;
using Domain.Books.Entities;

namespace Application.Core.BookFeatures.Query.QueryObjects;

public static class BookListDtoSelect
{
    /// <summary>
    /// select методы лутчше выносить в методы расширения. Паттерн "Объект-запрос".
    /// </summary>
    public static IQueryable<BookListDto> MapBookToDto(this IQueryable<Book> query)
    {
        return query.Select(b => new BookListDto()
        {
            BookId = b.Id,
            Title = b.Title,
            Price = b.Price,
            PublishedOn = b.PublishedOn,
            ActualPrice = b.Promotion == null ? b.Price : b.Promotion.NewPrice,
            PromotinalText = b.Promotion == null ? null : b.Promotion.PromotionalText,
            AuthorsOrdered = string.Join(", ",
                b.AuthorsLink
                    .OrderBy(ba => ba.Order)
                    .Select(ba => ba.Author.Name)),
            ReviewsCount = b.Reviews.Count,
            ReviewsAverageVotes = b.Reviews.Select(r => (double?) r.NumStars).Average(),
            TagStrings = b.Tags.Select(t => t.TagId).ToArray()
        });
    }
}