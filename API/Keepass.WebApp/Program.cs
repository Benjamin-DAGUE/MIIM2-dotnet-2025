using Keepass.WebApp.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//Lecture des informations de la WebAPI depuis le fichier de configuration.
string apiEndpoint = builder.Configuration.GetValue<string>("WebAPI:Endpoint") ?? throw new InvalidOperationException("WebAPI is not configured");
string apiScope = builder.Configuration.GetValue<string>("WebAPI:Scope") ?? throw new InvalidOperationException("WebAPI is not configured");

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
         .AddMicrosoftIdentityWebApp(builder.Configuration, "AzureAd")
            .EnableTokenAcquisitionToCallDownstreamApi([apiScope])
               .AddDownstreamApi("EntraIDAuthWebAPI", options =>
               {
                   options.BaseUrl = apiEndpoint;
                   options.Scopes = [apiScope];
               })
            .AddInMemoryTokenCaches();
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.MapGroup("/authentication").MapLoginAndLogout();

app.Run();
