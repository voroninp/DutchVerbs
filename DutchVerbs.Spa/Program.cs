using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using Blazored.LocalStorage;
using Blazored.LocalStorage.StorageOptions;
using DutchVerbs.Spa;
using DutchVerbs.Spa.Domain.Services;
using DutchVerbs.Spa.Infrastructure;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

Console.WriteLine($"Runtime: {RuntimeInformation.RuntimeIdentifier}");
Console.WriteLine($"Framework: {RuntimeInformation.FrameworkDescription}");
Console.WriteLine($"OS: {RuntimeInformation.OSArchitecture} - {RuntimeInformation.OSDescription}");
Console.WriteLine($"{nameof(RuntimeFeature.IsDynamicCodeSupported)}: {RuntimeFeature.IsDynamicCodeSupported}");
Console.WriteLine($"{nameof(RuntimeFeature.IsDynamicCodeCompiled)}: {RuntimeFeature.IsDynamicCodeCompiled}");

Console.WriteLine("Building application ...");

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IApplication, Application>();
builder.Services.Configure<LocalStorageOptions>(sp =>
{
    sp.JsonSerializerOptions.TypeInfoResolver = SourceGenerationContext.Default;
});
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBeforeUnload();

var app = builder.Build();
Console.WriteLine("Built");

Console.WriteLine("Initializing Application ...");
var state = app.Services.GetRequiredService<IApplication>();
Console.WriteLine("Acquired Application instance.");
await state.InitializeAsync();
Console.WriteLine("Initialized");
await app.RunAsync();