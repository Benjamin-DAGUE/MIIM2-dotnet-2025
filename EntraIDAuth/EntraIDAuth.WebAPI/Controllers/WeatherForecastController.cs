using EntraIDAuth.WebAPI.ObjectModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Data;

namespace EntraIDAuth.WebAPI.Controllers;


[Authorize]                                                         //Auhtorize au niveau du contr�leur donne le comportement par d�faut pour l'ensemble des routes. Ici, l'ensemble des routes , n�cessite un utilisateur connect� par d�faut.
[ApiController]                                                     //Permet d'indiquer que le contr�leur est une API.
[Route("[controller]")]                                             //La route du contr�leur va �tre nomm�e en fonction du nom de la classe
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]  //Le contr�leur n�cessite par d�faut l'acc�s avec un scope nomm� d�fini dans le fichier d� configuration.
public class WeatherForecastController() : ControllerBase
{
    #region Methods

    [HttpGet]
    [Authorize(Roles = Roles.User_Administrator)]       //La route va n�cessiter un r�le User ou Administrator.
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
    [Authorize(Roles = Roles.Administrator)]    //La route va n�cessiter un r�le Administrator.
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
    [AllowAnonymous]       //La route est publique et ne n�cessite pas un utilisateur connect�.
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
