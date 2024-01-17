namespace Domain.Persones;

public class Persone
{
    public PersoneName PersoneName { get; init; }
    public int Age { get; init; }

    public Persone(PersoneName personeName, string inn, string email, int age)
    {
        PersoneName = personeName;
        Age = age;
    }


    public static Persone? Create(PersoneName personeName, string? inn, string? email, int age)
    {
        if (string.IsNullOrEmpty(inn))
        {
            return null;
        }
        if (string.IsNullOrEmpty(email))
        {
            return null;
        }
        if (age < 18)
        {
            return null;
        }

        return new Persone(personeName, inn, email, age);
    }
}