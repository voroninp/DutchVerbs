using System.Collections.ObjectModel;

namespace DutchVerbs.Models;

public sealed class Verb
{
    public int Id { get; }
    public Language SourceLanguage { get; }
    public string SourceValue { get; }
    public Language TargetLanguage { get; }
    public IReadOnlyDictionary<VerbTime, string> TargetValueByTime { get; }

    public Verb(
        int id,
        Language sourceLanguage, string sourceValue,
        Language targetLanguage, IEnumerable<(VerbTime time, string value)> targetValues)
    {
        Id = id;
        SourceLanguage = sourceLanguage;
        SourceValue = sourceValue.Trim();
        TargetLanguage = targetLanguage;
        TargetValueByTime =
            new ReadOnlyDictionary<VerbTime, string>(
                new SortedDictionary<VerbTime, string>(
                    targetValues.ToDictionary(v => v.time, v => v.value)));
    }
}
