namespace DutchVerbs.Spa.Domain.Models;

public sealed record LearningProgress(
    int VerbId,
    LastAnswer? LastAnswer, int PresentationsCount, int CorrectAnswers,
    DateTimeOffset LastPresentationMoment);

public sealed record LastAnswer(
    bool IsCorrect,
    int? ErrorPosition);