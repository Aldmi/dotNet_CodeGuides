using Domain.Core.FilmsAgregate;

namespace Domain.Core.StoreAgregate;

public class Inventory
{
    public int InventoryId { get; private set; }
    public DateTime LastUpdate { get; private set; }
    public int StoreId { get; set; }
    
    public Film Film { get; set; }
}