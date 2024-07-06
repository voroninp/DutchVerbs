using DutchVerbs.Models;

namespace DutchVerbs;

public interface IApplication
{
    public IReadOnlyDictionary<int, Verb> VerbById { get; }

    public IReadOnlyDictionary<int, LearningProgress> LearningProgressByVerbId { get; }

    ValueTask InitializeAsync();

    ValueTask PersistState();

    Verb GetNextVerb();

    LearningProgress VerifyAnswerForVerb(Verb verb, string answer);
}
