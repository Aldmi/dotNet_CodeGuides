using Domain.Core.CustomerAgregate;
using Domain.Core.StaffAgregate;

namespace Domain.Core.StoreAgregate;

public class Rental
{
    public int RentalId { get; private set; }
    public DateTime RentalDate { get; init; }
    public DateTime? ReturnDate { get; init; }
    public DateTime LastUpdate { get; init; }

    public Staff Staff { get; set; }  //Кто выдал товар из сотрудников
    public Customer Customer { get; set; }  //Клиент взявший товар
    public Inventory Inventory { get; set; } //Товар
}