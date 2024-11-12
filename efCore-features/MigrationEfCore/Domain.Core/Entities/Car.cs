using CSharpFunctionalExtensions;

namespace Domain.Core.Entities;

public class Car : ValueObject<Car>
{
    public string ModelName { get; set; }
    public string? RegisterNumber { get; set; }
    public DateTime YearManufacture { get; set; }
    
    
    protected override bool EqualsCore(Car other)
    {
        return ModelName == other.ModelName &&
               RegisterNumber == other.RegisterNumber &&
               YearManufacture == other.YearManufacture;
    }

    protected override int GetHashCodeCore()
    {
        return ModelName.GetHashCode() + RegisterNumber.GetHashCode() + YearManufacture.GetHashCode();
    }
}