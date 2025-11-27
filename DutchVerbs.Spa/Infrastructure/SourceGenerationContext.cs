using System.Text.Json.Serialization;
using DutchVerbs.Spa.Infrastructure.DTOs;

namespace DutchVerbs.Spa.Infrastructure;

[JsonSerializable(typeof(ApplicationStateDto))]
[JsonSerializable(typeof(object))]
[JsonSourceGenerationOptions(WriteIndented = true)]
public sealed partial class SourceGenerationContext : JsonSerializerContext
{
}