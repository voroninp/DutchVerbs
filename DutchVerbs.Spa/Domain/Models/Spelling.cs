namespace DutchVerbs.Spa.Domain.Models;

public readonly record struct Spelling(string Value)
{
    public string Value { get; } = Value.Trim();

    public static implicit operator string(Spelling spelling) => spelling.Value;

    public override string ToString() => Value;
}
