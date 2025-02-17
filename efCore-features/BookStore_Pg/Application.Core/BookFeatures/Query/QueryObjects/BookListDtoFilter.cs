﻿using Application.Core.BookFeatures.Query.ListBooksQuery;

namespace Application.Core.BookFeatures.Query.QueryObjects;

public enum BooksFilterBy
{
    NoFilter = 0,
    ByVotes,
    ByTags,
    ByPublicationYear
}

public static class BookListDtoFilter
{
    public const string AllBooksNotPublishedString = "Coming Soon";

    public static IQueryable<BookListDto> FilterBooksBy(this IQueryable<BookListDto> books, BooksFilterBy filterBy, string filterValue) //#A
    {
        if (string.IsNullOrEmpty(filterValue)) //#B
            return books; //#B

        switch (filterBy)
        {
            case BooksFilterBy.NoFilter: //#C
                return books; //#C
            
            case BooksFilterBy.ByVotes:
                var filterVote = int.Parse(filterValue); //#D
                return books.Where(x => //#D
                    x.ReviewsAverageVotes > filterVote); //#D
            
            case BooksFilterBy.ByTags:
                return books.Where(x => x.TagStrings //#E
                    .Any(y => y == filterValue)); //#E
            
            case BooksFilterBy.ByPublicationYear:
                if (filterValue == AllBooksNotPublishedString) //#F
                    return books.Where( //#F
                        x => x.PublishedOn > GetDateOnlyNowUtc()); //#F

                var filterYear = int.Parse(filterValue); //#G
                return books.Where( //#G
                    x => x.PublishedOn.Year == filterYear //#G
                         && x.PublishedOn <= GetDateOnlyNowUtc()); //#G
            default:
                throw new ArgumentOutOfRangeException
                    (nameof(filterBy), filterBy, null);
        }
    }


    private static DateOnly GetDateOnlyNowUtc() => DateOnly.FromDateTime(DateTime.UtcNow);

    /***************************************************************
    #A The method is given both the type of filter and the user selected filter value
    #B If the filter value isn't set then it returns the IQueryable with no change
    #C Same for no filter selected - it returns the IQueryable with no change
    #D The filter by votes is a value and above, e.g. 3 and above. Note: not reviews returns null, and the test is always false
    #E This will select any books that have a Tag category that matches the filterValue
    #F If the "coming soon" was picked then we only return books not yet published
    #G If we have a specific year we filter on that. Note that we also remove future books (in case the user chose this year's date)
     * ************************************************************/
}

