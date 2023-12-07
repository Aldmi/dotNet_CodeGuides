using System.Reflection;

namespace SmartEnum;

public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    //static поле инициализируется только после ПЕРВОГО обращения.
    private static readonly Dictionary<int, TEnum> Enumerations = CreateEnumerations();
    
    public int Value { get; protected init; }
    public string Name { get; protected init; }


    protected Enumeration(int value, string name)
    {
        Value = value;
        Name = name;
    }
    
    
    public static TEnum? FromValue(int value)
    {
        return Enumerations.
            TryGetValue(value, out var enumeration) ? enumeration : default;
    }
    
    public static TEnum? FromName(string name)
    {
        return Enumerations
            .Values
            .SingleOrDefault(e => e.Name == name);
    }
    
    
    
    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null)
            return false;

        return GetType() == other.GetType() &&
               Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Enumeration<TEnum> other 
               && Equals(other);
    }


    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }


    public override string ToString()
    {
        return Name;
    }
    
    
    private static Dictionary<int, TEnum> CreateEnumerations()
    {
        var enumType = typeof(TEnum);
        var fieldsForType =
            enumType
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fieldInfo => enumType.IsAssignableFrom(fieldInfo.FieldType))
                .Select(fieldInfo => (TEnum) fieldInfo.GetValue(default)!);

        return fieldsForType.ToDictionary(x => x.Value);
    }
}