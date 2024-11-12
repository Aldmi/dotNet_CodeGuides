namespace Domain.Core.AddressAgregate;

public class City
{
    public int CityId { get; private set; }
    public string Name { get; private set; }
    public DateTime LastUpdate { get; private set; }
    
    
    // public int CountryIdFk { get; set; }
    //public Country Country { get; private set; }
}