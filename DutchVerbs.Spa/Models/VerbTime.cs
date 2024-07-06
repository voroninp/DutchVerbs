namespace DutchVerbs.Models;

public sealed record VerbTime : IComparable<VerbTime>
{
    private static readonly Dictionary<int, VerbTime> VerbTimeById = new();
    public static VerbTime ById(int id)
    {
        if (VerbTimeById.TryGetValue(id, out var time))
        {
            return time;
        }

        throw new KeyNotFoundException(@"There's no verb time with id '{id}'.");
    }

    public int CompareTo(VerbTime? other)
    {
        if (other is null)
        {
            return -1;
        }

        return Id.CompareTo(other.Id);
    }

    public int Id { get; }

    private VerbTime(int id)
    {
        Id = id;

        VerbTimeById.Add(Id, this);
    }

    public static readonly VerbTime PastSimple = new VerbTime(1);
    public static readonly VerbTime Perfect = new VerbTime(2);
    public static readonly VerbTime Present = new VerbTime(3);
}
