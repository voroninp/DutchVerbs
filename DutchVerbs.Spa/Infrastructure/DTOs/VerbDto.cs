using DutchVerbs.Spa.Domain.Models;

namespace DutchVerbs.Spa.Infrastructure.DTOs;

public sealed record VerbDto(
    int Id,
    int SourceLanguageId, string SourceValue,
    int TargetLanguageId, Dictionary<int, string> TargetValueByTimeId)
{
    public static VerbDto FromModel(VerbMapping verb)
        =>
        new VerbDto(
            verb.Id,
            verb.SourceLanguage.Id, verb.SourceValue,
            verb.TargetLanguage.Id, verb.TargetValueByTime.ToDictionary(kvp => kvp.Key.Id, kvp => kvp.Value.Value));

    public VerbMapping ToModel()
        =>
        new VerbMapping(
            Id,
            Language.ById(SourceLanguageId), new Spelling(SourceValue),
            Language.ById(TargetLanguageId), TargetValueByTimeId.Select(kvp => (VerbTime.ById(kvp.Key), new Spelling(kvp.Value))));
}