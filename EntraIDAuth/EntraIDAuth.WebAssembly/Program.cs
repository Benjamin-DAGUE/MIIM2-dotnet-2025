using EntraIDAuth.WebAssembly;
using EntraIDAuth.WebAssembly.Core;
using EntraIDAuth.WebAssembly.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

//Demande d'instancier les composants
//  - App.razor dans la div qui porte l'identifiant app (voir wwwroot/index.html)
//  - HeadOutlet � la suite de head (voir wwwroot/index.html). Cela permet d'injecter depuis d'autre composant des �l�ments dans <head></head>
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//Lecture des informations de la WebAPI depuis le fichier de configuration.
string apiEndpoint = builder.Configuration.GetValue<string>("WebAPI:Endpoint") ?? throw new InvalidOperationException("WebAPI is not configured");
string apiScope = builder.Configuration.GetValue<string>("WebAPI:Scope") ?? throw new InvalidOperationException("WebAPI is not configured");

//Ajout de MSAL qui est la biblioth�que d'authentification aupr�s de Microsoft Entra ID (anciennement Azure AD).
builder.Services
    .AddMsalAuthentication(options =>
    {
        builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
        //Pr�cise le scope de l'API pour laquel on souhaite obtenir un jeton d'identification.
        options.ProviderOptions.DefaultAccessTokenScopes.Add(apiScope);
    })
    //L'ajout d'une Factory permet d'avoir la main sur l'utilisateur lors de sa connexion. Voir le commentaire de la classe pour plus de d�tail.
    .AddAccountClaimsPrincipalFactory<CustomAccountClaimsPrincipalFactory>();

//Ajout d'un service WebAPIClient qui d�pend d'un HttpClient
builder.Services
    .AddHttpClient<WebAPIClient>(client => client.BaseAddress = new Uri(apiEndpoint))
    //Le HttpClient pr�cise l'utilisation d'un MessageHandler qui se charge de transf�rer le jeton d'identification de l'utilisateur connect�.
    .AddHttpMessageHandler(sp =>
    {
        //La classe AuthorizationMessageHandler se charge de transf�rer le token d'identification � l'API.
        AuthorizationMessageHandler handler = sp.GetRequiredService<AuthorizationMessageHandler>()
            .ConfigureHandler(
                authorizedUrls: [apiEndpoint],
                scopes: [apiScope]);

        return handler;
    });

await builder.Build().RunAsync();
