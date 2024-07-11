namespace DutchVerbs.Spa.Infrastructure.DTOs;

public sealed record ApplicationStateDto(VerbDto[] Verbs, LearningProgressDto[] Learnings);
