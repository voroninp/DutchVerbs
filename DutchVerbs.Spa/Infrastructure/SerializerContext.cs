using System.Text.Json.Serialization;
using DutchVerbs.Spa.Infrastructure.DTOs;

namespace DutchVerbs.Spa.Infrastructure;

[JsonSerializable(typeof(ApplicationStateDto))]
[JsonSourceGenerationOptions(WriteIndented = true)]
public sealed partial class SourceGenerationContext : System.Text.Json.Serialization.JsonSerializerContext
{
}
