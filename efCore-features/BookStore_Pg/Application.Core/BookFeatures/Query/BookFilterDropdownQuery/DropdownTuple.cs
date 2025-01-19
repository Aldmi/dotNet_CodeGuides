
namespace Application.Core.BookFeatures.Query.BookFilterDropdownQuery;

public class DropdownTuple
{
    public string Value { get; set; }

    public string Text { get; set; }

    public override string ToString()
    {
        return $"{nameof(Value)}: {Value}, {nameof(Text)}: {Text}";
    }
}