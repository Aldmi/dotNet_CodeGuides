using Domain.Core.AddressAgregate;

namespace Domain.Core.CustomerAgregate;

public class Customer
{
    public int CustomerId { get; private set; }
    public int StoreId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public bool ActiveBool { get; set; }
    public DateTime CreateDate { get; init; }
    public DateTime LastUpdate { get; init; }
    public int Active { get; set; }
    public Address Address { get; set; }
}