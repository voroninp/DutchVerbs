using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using DutchVerbs.Spa;
using DutchVerbs.Spa.Domain.Services;
using DutchVerbs.Spa.Infrastructure;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Text.Json;

Console.WriteLine($"Runtime: {RuntimeInformation.RuntimeIdentifier}");
Console.WriteLine($"Framework: {RuntimeInformation.FrameworkDescription}");
Console.WriteLine($"OS: {RuntimeInformation.OSArchitecture} - {RuntimeInformation.OSDescription}");
Console.WriteLine($"{nameof(RuntimeFeature.IsDynamicCodeSupported)}: {RuntimeFeature.IsDynamicCodeSupported}");
Console.WriteLine($"{nameof(RuntimeFeature.IsDynamicCodeCompiled)}: {RuntimeFeature.IsDynamicCodeCompiled}");

Console.WriteLine("Checking serialization ...");
var json = JsonSerializer.Serialize(
    new SerializationCheck("xyz"),
    new JsonSerializerOptions
    {
        TypeInfoResolver = SourceGenerationContext.Default,
    });
Console.WriteLine($"Successfully serialized json: {json}.");

Console.WriteLine("Initializing ...");

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IApplication, Application>();
builder.Services.AddBlazoredLocalStorage(cfg =>
{
    cfg.JsonSerializerOptions.TypeInfoResolver = SourceGenerationContext.Default;
});
builder.Services.AddBeforeUnload();

var app = builder.Build();

var state = app.Services.GetRequiredService<IApplication>();
await state.InitializeAsync();
Console.WriteLine("Initialized");
await app.RunAsync();