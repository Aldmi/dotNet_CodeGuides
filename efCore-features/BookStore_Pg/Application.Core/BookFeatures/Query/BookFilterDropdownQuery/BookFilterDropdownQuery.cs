﻿using Application.Core.Abstract;
using Application.Core.BookFeatures.Query.QueryObjects;

namespace Application.Core.BookFeatures.Query.BookFilterDropdownQuery;

public class BookFilterDropdownQuery(IBookContext _db)
{
    /// <summary>
    ///     This makes the various Value + text to go in the dropdown based on the FilterBy option
    /// </summary>
    public IEnumerable<DropdownTuple> GetFilterDropDownValues(BooksFilterBy filterBy)
    {
        switch (filterBy)
        {
            case BooksFilterBy.NoFilter:
                //return an empty list
                return new List<DropdownTuple>();
            case BooksFilterBy.ByVotes:
                return FormVotesDropDown();
            case BooksFilterBy.ByTags:
                return _db.Tags
                    .Select(x => new DropdownTuple
                    {
                        Value = x.TagId,
                        Text = x.TagId
                    }).ToList();
            case BooksFilterBy.ByPublicationYear:

                var today = DateOnly.FromDateTime(DateTime.UtcNow); //#A
                var result = _db.Books //#B
                    .Where(x => x.PublishedOn <= today) //#B
                    .Select(x => x.PublishedOn.Year) //#B
                    .Distinct() //#B
                    .OrderByDescending(x => x) //#C
                    .Select(x => new DropdownTuple //#D
                    {
                        //#D
                        Value = x.ToString(), //#D
                        Text = x.ToString() //#D
                    }).ToList(); //#D
                var comingSoon = _db.Books. //#E
                    Any(x => x.PublishedOn > today); //#E
                if (comingSoon) //#F
                    result.Insert(0, new DropdownTuple //#F
                    {
                        Value = BookListDtoFilter.AllBooksNotPublishedString,
                        Text = BookListDtoFilter.AllBooksNotPublishedString
                    });

                return result;
            /*****************************************************************
            #A Today's date so we can filer out books that haven't be published yet
            #B This long command gets the year of publication by filters out the future books, select the date and uses distinct to only have one of each year
            #C Orders the years, with newest year at the top
            #D I finally use two client/server evaluations to turn the values into strings
            #E This returns true if there is a book in the list that is not yet published
            #F Finally I add a "coming soon" filter for all the future books
             * ***************************************************************/
            default:
                throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null);
        }
    }


    private static IEnumerable<DropdownTuple> FormVotesDropDown()
    {
        return new[]
        {
            new DropdownTuple {Value = "4", Text = "4 stars and up"},
            new DropdownTuple {Value = "3", Text = "3 stars and up"},
            new DropdownTuple {Value = "2", Text = "2 stars and up"},
            new DropdownTuple {Value = "1", Text = "1 star and up"},
        };
    }
}