using Domain.Core.AddressAgregate;

namespace Domain.Core.StaffAgregate;

public class Staff
{
    public int StuffId { get; private set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public int StoreId { get; set; }
    public bool ActiveBool { get; set; }
    public string UserName { get; set; }
    public string? Password { get; set; }
    public DateTime LastUpdate { get; init; }
    public byte[]? Picture { get; set; }
    
    public Address Address { get; set; }
}