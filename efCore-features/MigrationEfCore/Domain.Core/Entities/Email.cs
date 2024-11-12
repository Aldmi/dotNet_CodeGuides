using CSharpFunctionalExtensions;

namespace Domain.Core.Entities;

public class Email : ValueObject<Email>
{
    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; set; }



    public static Result<Email> Create(string emailAddress)
    {
        if(string.IsNullOrEmpty(emailAddress))
            return Result.Failure<Email>("emailAddress не может быть пуст");
        
        // if(emailAddress.Split('@').Length == 2)
        //     return Result.Failure<Email>("@ не указанна");
        
        return new Email(emailAddress);
    }
    
    
    protected override bool EqualsCore(Email other)
    {
        return Value == other.Value;
    }

    protected override int GetHashCodeCore()
    {
        return Value.GetHashCode() * 397;
    }
}