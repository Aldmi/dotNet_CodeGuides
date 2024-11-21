
namespace Domain.Books.Entities
{
    public class PriceOffer //#A
    {
        public long PriceOfferId { get; set; }
        public decimal NewPrice { get; set; }
        public string PromotionalText { get; set; }

        //-----------------------------------------------
        //Relationships

        public long BookId { get; set; } //#b
    }

    /***************************************************
    #N The PriceOffer is designed to override the normal price. It is a One-to-ZeroOrOne relationhsip
    #O This foreign key links back to the book it should be applied to
     Создается уникальный индекс BookId, чтобы у одной книги был только один PriceOffer "create unique index "IX_PriceOffers_BookId" on "PriceOffers" ("BookId");"
     * *************************************************/
}