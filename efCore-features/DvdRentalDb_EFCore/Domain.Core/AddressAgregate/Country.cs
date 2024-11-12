namespace Domain.Core.AddressAgregate;

public class Country
{
    public int CountryId { get; private set; }
    public string Value { get; private set; }
    public DateTime LastUpdate { get; private set; }
    
    public List<City> Cities { get; private set;  } = new();
}