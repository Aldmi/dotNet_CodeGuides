using Domain.Core.AddressAgregate;
using Domain.Core.StaffAgregate;

namespace Domain.Core.StoreAgregate;

public class Store
{
    public int StoreId { get; set; }
    public DateTime LastUpdate { get; init; }

    public Staff ManagerStaff { get; set; }
    public Address Address { get; set; }
}