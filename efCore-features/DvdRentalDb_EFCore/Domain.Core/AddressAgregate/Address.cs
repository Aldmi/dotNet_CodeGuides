using Domain.Core.CustomerAgregate;

namespace Domain.Core.AddressAgregate;

public class Address
{
    public int AddressId { get; private set; }
    
    public string Address1 { get; private set; }
    public string? Address2 { get; private set; }
    public string District { get; private set; }
    public string? PostalCode { get; private set; }
    public string Phone { get; private set; }
    public DateTime LastUpdate { get; private set; }
    public City City { get; set; }
}