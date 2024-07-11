namespace DutchVerbs.Spa.Domain.Models;

public sealed record Language
{
    private static readonly Dictionary<int, Language> LanguageById = new();
    public static Language ById(int id)
    {
        if (LanguageById.TryGetValue(id, out var language))
        {
            return language;
        }

        throw new KeyNotFoundException(@"There's no language with id '{id}'.");
    }

    public int Id { get; }
    public string LocaleCode { get; }
    public string CountryCode { get; }
    public string Code { get; }

    private Language(int id, string localeCode, string countryCode)
    {
        Id = id;
        LocaleCode = localeCode.Trim().ToLower();
        CountryCode = countryCode.Trim().ToUpper();
        Code = $"{LocaleCode}-{CountryCode}";

        LanguageById.Add(id, this);
    }

    public static readonly Language Russian_Russia = new Language(1, "ru", "RU");
    public static readonly Language Dutch_Netherlands = new Language(2, "nl", "NL");
}
