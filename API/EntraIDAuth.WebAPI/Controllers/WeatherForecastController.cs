using EntraIDAuth.WebAPI.ObjectModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Data;

namespace EntraIDAuth.WebAPI.Controllers;


[Authorize]                                                         //Auhtorize au niveau du contrôleur donne le comportement par défaut pour l'ensemble des routes. Ici, l'ensemble des routes , nécessite un utilisateur connecté par défaut.
[ApiController]                                                     //Permet d'indiquer que le contrôleur est une API.
[Route("[controller]")]                                             //La route du contrôleur va être nommée en fonction du nom de la classe
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]  //Le contrôleur nécessite par défaut l'accès avec un scope nommé défini dans le fichier dé configuration.
public class WeatherForecastController() : ControllerBase
{
    #region Methods

    [HttpGet]
    [Authorize(Roles = Roles.User_Administrator)]       //La route va nécessiter un rôle User ou Administrator.
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = WeatherForecast.Summaries[Random.Shared.Next(WeatherForecast.Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("onlyadmin")]
    [Authorize(Roles = Roles.Administrator)]    //La route va nécessiter un rôle Administrator.
    public IEnumerable<WeatherForecast> GetForAdmin()
    {
        return Enumerable.Range(1, 10).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = WeatherForecast.Summaries[Random.Shared.Next(WeatherForecast.Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("anonymous")]
    [AllowAnonymous]       //La route est publique et ne nécessite pas un utilisateur connecté.
    public IEnumerable<WeatherForecast> GetAnonymous()
    {
        return Enumerable.Range(1, 1).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = WeatherForecast.Summaries[Random.Shared.Next(WeatherForecast.Summaries.Length)]
        })
        .ToArray();
    }

    #endregion
}
