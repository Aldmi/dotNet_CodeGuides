namespace Domain.Core.Entities;

public class Persone
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public Address Address { get; set; }
    public Email Email { get; set; }
    public List<Car>? Cars { get; set; }
}