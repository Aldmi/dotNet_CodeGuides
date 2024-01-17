namespace Domain.Persones;

public class PersoneName
{
    private const int DefaultLenght = 2;
    
    private PersoneName(string value) => Value = value;
    
    public string Value { get; init; }
    
    public string AliasValue { get; private set; }


    public static PersoneName? Create(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        if (value.Length != DefaultLenght)
        {
            return null;
        }

        return new PersoneName(value);
    }


    public bool SetAliasValue(string aliasValue)
    {
        if (!aliasValue.StartsWith("alias_"))
        {
            return false;
        }
        
        AliasValue = aliasValue;
        return true;
    }
}