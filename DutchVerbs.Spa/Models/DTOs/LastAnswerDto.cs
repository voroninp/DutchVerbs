namespace DutchVerbs.Models.DTOs;

public sealed record LastAnswerDto(
    bool IsCorrect,
    int? ErrorPosition)
{
    public static LastAnswerDto? FromModel(LastAnswer? model) 
        =>
        model switch
        {
            null => null,
            _ => new LastAnswerDto(model.IsCorrect, model.ErrorPosition)
        };

    public LastAnswer ToModel() => new LastAnswer(IsCorrect, ErrorPosition);
}

