using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using DutchVerbs.Spa;
using DutchVerbs.Spa.Domain.Services;
using DutchVerbs.Spa.Infrastructure;

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