using DutchVerbs.Spa.Domain.Models;

namespace DutchVerbs.Spa.Domain.Services;

public interface IApplication
{
    public IReadOnlyDictionary<int, VerbMapping> VerbById { get; }

    public IReadOnlyDictionary<int, LearningProgress> LearningProgressByVerbId { get; }

    ValueTask InitializeAsync();

    ValueTask PersistState();

    VerbMapping GetNextVerb();

    LearningProgress VerifyAnswerForVerb(VerbMapping verb, string answer);
}
