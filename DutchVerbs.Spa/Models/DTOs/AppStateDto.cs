namespace DutchVerbs.Models.DTOs;

public sealed record AppStateDto(VerbDto[] Verbs, LearningProgressDto[] Learnings);
