using HTLVBFingerflitzer.Web.Components;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddSingleton<IDailyChallengeTextGenerator, DailyChallengeTextGenerator>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();



public interface IDailyChallengeTextGenerator
{
    Task<string> GetDailyChallengeTextAsync();
}


public class DailyChallengeTextGenerator : IDailyChallengeTextGenerator
{
    private readonly string _text;

    public DailyChallengeTextGenerator(IConfiguration configuration)
    {
        _text = configuration.GetSection("DailyChallenge")["StaticText"]
                ?? "Fallback Text, falls keine Konfiguration vorhanden ist.";
    }

    public Task<string> GetDailyChallengeTextAsync()
    {
        return Task.FromResult(_text);
    }
}
