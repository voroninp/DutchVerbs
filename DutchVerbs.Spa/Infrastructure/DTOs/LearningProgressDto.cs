using DutchVerbs.Spa.Domain.Models;

namespace DutchVerbs.Spa.Infrastructure.DTOs;

public sealed record LearningProgressDto(
    int VerbId,
    LastAnswerDto? LastAnswer, int PresentationsCount, int CorrectAnswers,
    DateTimeOffset LastPresentationMoment)
{
    public static LearningProgressDto FromModel(LearningProgress learningProgress)
        =>
        new LearningProgressDto(
            learningProgress.VerbId,
            LastAnswerDto.FromModel(learningProgress.LastAnswer),
            learningProgress.PresentationsCount, learningProgress.CorrectAnswers,
            learningProgress.LastPresentationMoment);

    public LearningProgress ToModel()
        =>
        new LearningProgress(VerbId, LastAnswer?.ToModel(), PresentationsCount, CorrectAnswers, LastPresentationMoment);
}