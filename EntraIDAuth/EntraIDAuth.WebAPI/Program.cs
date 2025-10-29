using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

string? corsFrontEndpoint = builder.Configuration.GetValue<string>("CorsFrontEndpoint");

//CORS permet d'autoriser le front à appeler l'API.
if (string.IsNullOrWhiteSpace(corsFrontEndpoint) == false)
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("WebAssemblyOrigin", policy =>
        {
            policy
                .WithOrigins(corsFrontEndpoint)
                .AllowAnyMethod()
                .WithHeaders(HeaderNames.ContentType, HeaderNames.Authorization, "x-custom-header")
                .AllowCredentials();
        });
    });
}

//Ajout de l'authentification Entra ID
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

//Indique que l'API va être développé à l'aide de Controllers.
builder.Services.AddControllers();

var app = builder.Build();

//Redirige les utilisateurs http vers https
app.UseHttpsRedirection();

//Indique la mise en place de la police CORS créée précédement.
if (string.IsNullOrWhiteSpace(corsFrontEndpoint) == false)
{
    app.UseCors("WebAssemblyOrigin");
}

//Ajout la couche d'authentification
app.UseAuthentication();

//Ajoute la couche d'autorisation
app.UseAuthorization();

//Indique de créer les routes pour les Controlles.
app.MapControllers();

app.Run();
