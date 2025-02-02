using FilmFront.Components;
using FilmFront.Components.SharedModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace FilmFront;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        builder.Services.AddAuthenticationCore();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<FilmService>();
        builder.Services.AddScoped<OmdbService>();
        builder.Services.AddScoped<AuthProvider>();
        builder.Services.AddScoped<FavorisService>();
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["ApiBaseAddress"]) });

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseStaticFiles();
        app.UseAntiforgery();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
