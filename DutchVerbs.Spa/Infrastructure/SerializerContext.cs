using System.Text.Json.Serialization;
using DutchVerbs.Spa.Infrastructure.DTOs;

namespace DutchVerbs.Spa.Infrastructure;

[JsonSerializable(typeof(ApplicationStateDto))]
[JsonSerializable(typeof(SerializationCheck))]
[JsonSourceGenerationOptions(WriteIndented = true)]
public sealed partial class SourceGenerationContext : JsonSerializerContext
{
}

public sealed record SerializationCheck(string Property);