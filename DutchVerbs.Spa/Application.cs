using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using blazejewicz.Blazor.BeforeUnload;
using Blazored.LocalStorage;
using DutchVerbs.Models;
using DutchVerbs.Models.DTOs;

namespace DutchVerbs;

public sealed class Application : IApplication
{
    private static readonly string AppStateKey = "AppState";

    public static readonly StringSplitOptions TrimAndRemoveEmpty = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;

    private readonly HttpClient _httpClient;
    private readonly ISyncLocalStorageService _storage;
    private readonly BeforeUnload _beforeUnload;

    private readonly Dictionary<int, Verb> _verbById = new();
    private readonly Dictionary<int, LearningProgress> _learningProgressByVerbId = new();

    public IReadOnlyDictionary<int, Verb> VerbById { get; }
    public IReadOnlyDictionary<int, LearningProgress> LearningProgressByVerbId { get; }

    public Application(HttpClient httpClient, ISyncLocalStorageService storage, BeforeUnload beforeUnload)
    {
        _httpClient = httpClient;
        _storage = storage;
        _beforeUnload = beforeUnload;

        VerbById = new ReadOnlyDictionary<int, Verb>(_verbById);
        LearningProgressByVerbId = new ReadOnlyDictionary<int, LearningProgress>(_learningProgressByVerbId);
    }

    private bool TryGetStateFromStorage([NotNullWhen(true)] out AppStateDto? stateDto)
    {
        if (!_storage.ContainKey(AppStateKey))
        {
            Console.WriteLine($"Storage has no item with key ;{AppStateKey}'.");

            stateDto = null;
            return false;
        }

        try
        {
            stateDto = _storage.GetItem<AppStateDto>(AppStateKey)!;
            Debug.Assert(stateDto != null);

            if (stateDto.Verbs.Length == 0)
            {
                Console.WriteLine("State seems to be broken, because it contains zero verbs.");

                stateDto = null;
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Got an exception while deserializing state: {0}", ex);

            stateDto = null;
            return false;
        }
    }

    public async ValueTask InitializeAsync()
    {
        if (!TryGetStateFromStorage(out var stateDto))
        {
            await BuildFreshState();
            await PersistState();
            TryGetStateFromStorage(out stateDto);
        }

        if (stateDto is null)
        {
            throw new Exception("Could not initialize application state.");
        }

        _verbById.Clear();
        foreach (var verb in stateDto.Verbs)
        {
            _verbById.Add(verb.Id, verb.ToModel());
        }

        _learningProgressByVerbId.Clear();
        foreach (var progress in stateDto.Learnings)
        {
            _learningProgressByVerbId.Add(progress.VerbId, progress.ToModel());
        }

        _beforeUnload.BeforeUnloadHandler += OnUnload;

        Console.WriteLine("Initialized Application state.");
    }

    private static readonly Regex Word = new Regex(@"\w+-?\w+", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.NonBacktracking);

    private async Task BuildFreshState()
    {
        _verbById.Clear();
        _learningProgressByVerbId.Clear();

        var response = await _httpClient.GetAsync("verbs-ru-nl.txt");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var lines = content.Split(new[] { '\n', '\r' }, TrimAndRemoveEmpty);
        var id = 0;

        _verbById.Clear();
        var verbs = lines.Select(line =>
        {
            var words = line.Split(';', TrimAndRemoveEmpty);

            var sourceVerb = words[0];

            var targetVerbs = new[]
            {
                (VerbTime.Present, Word.Match(words[1]).Value),
                (VerbTime.PastSimple, Word.Match(words[2]).Value),
                (VerbTime.Perfect, Word.Match(words[3]).Value)
            };

            return new Verb(
                    ++id,
                    Language.Russian_Russia, sourceVerb,
                    Language.Dutch_Netherlands, targetVerbs);
        });

        foreach (var verb in verbs)
        {
            _verbById.Add(verb.Id, verb);

            var progress = new LearningProgress(verb.Id, LastAnswer: null, PresentationsCount: 0, CorrectAnswers: 0, DateTimeOffset.MinValue);
            _learningProgressByVerbId.Add(verb.Id, progress);
        }

        Console.WriteLine("Built fresh state.");
    }

    private void OnUnload(object? sender, EventArgs eventArgs)
    {
        PersistState().GetAwaiter().GetResult();
    }

    public async ValueTask PersistState()
    {
        var state = new AppStateDto(
            VerbById.Values.Select(VerbDto.FromModel).ToArray(),
            LearningProgressByVerbId.Values.Select(LearningProgressDto.FromModel).ToArray());

        _storage.SetItem(AppStateKey, state);
    }

    public Verb GetNextVerb()
    {
        var index = Random.Shared.Next(VerbById.Count);
        return VerbById.Values.Skip(index - 1).First();
    }

    public LearningProgress VerifyAnswerForVerb(Verb verb, string answer)
    {
        var words = answer.Split(" ", TrimAndRemoveEmpty).Select(w => w.ToLower()).ToList();

        var lastAnswerWasCorrect =
            words.Count == verb.TargetValueByTime.Count
            && words.Zip(verb.TargetValueByTime.Values).All(t => t.First == t.Second);

        var lastAnswer = new LastAnswer(lastAnswerWasCorrect, ErrorPosition: lastAnswerWasCorrect ? null : 0);

        var progress = _learningProgressByVerbId.GetValueOrDefault(
            verb.Id, new LearningProgress(verb.Id, LastAnswer: null, PresentationsCount:0, CorrectAnswers: 0, DateTimeOffset.MinValue));

        progress = progress with
        {
            LastAnswer = lastAnswer,
            PresentationsCount = progress.PresentationsCount + 1,
            CorrectAnswers = progress.CorrectAnswers + Convert.ToInt32(lastAnswerWasCorrect),
            LastPresentationMoment = DateTimeOffset.UtcNow
        };

        _learningProgressByVerbId[progress.VerbId] = progress;

        return progress;
    }
}