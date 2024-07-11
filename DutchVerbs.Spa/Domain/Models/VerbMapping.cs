using System.Collections.ObjectModel;

namespace DutchVerbs.Spa.Domain.Models;

public sealed class VerbMapping
{
    public int Id { get; }
    public Language SourceLanguage { get; }
    public Spelling SourceValue { get; }
    public Language TargetLanguage { get; }
    public IReadOnlyDictionary<VerbTime, Spelling> TargetValueByTime { get; }

    public VerbMapping(
        int id,
        Language sourceLanguage, Spelling sourceValue,
        Language targetLanguage, IEnumerable<(VerbTime time, Spelling value)> targetValues)
    {
        Id = id;
        SourceLanguage = sourceLanguage;
        SourceValue = sourceValue;
        TargetLanguage = targetLanguage;
        TargetValueByTime =
            new ReadOnlyDictionary<VerbTime, Spelling>(
                new SortedDictionary<VerbTime, Spelling>(
                    targetValues.ToDictionary(v => v.time, v => v.value)));
    }
}
