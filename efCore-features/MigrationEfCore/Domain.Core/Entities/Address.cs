namespace Domain.Core.Entities;

public class Address
{
    public Guid Id { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}