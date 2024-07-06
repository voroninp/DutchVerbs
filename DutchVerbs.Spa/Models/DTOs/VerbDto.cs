using DutchVerbs.Models;

namespace DutchVerbs.Models.DTOs;

public sealed record VerbDto(
    int Id,
    int SourceLanguageId, string SourceValue,
    int TargetLanguageId, Dictionary<int, string> TargetValueByTimeId)
{
    public static VerbDto FromModel(Verb verb)
        =>
        new VerbDto(
            verb.Id,
            verb.SourceLanguage.Id, verb.SourceValue,
            verb.TargetLanguage.Id, verb.TargetValueByTime.ToDictionary(kvp => kvp.Key.Id, kvp => kvp.Value));

    public Verb ToModel()
        =>
        new Verb(
            Id,
            Language.ById(SourceLanguageId), SourceValue,
            Language.ById(TargetLanguageId), TargetValueByTimeId.Select(kvp => (VerbTime.ById(kvp.Key), kvp.Value)));
}