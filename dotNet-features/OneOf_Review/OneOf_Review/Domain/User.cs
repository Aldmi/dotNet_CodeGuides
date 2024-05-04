namespace OneOf_Review.Domain;

public class User
{
    public User(string name)
    {
        Name = name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
}