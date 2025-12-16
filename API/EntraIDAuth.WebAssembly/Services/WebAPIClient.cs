using EntraIDAuth.WebAPI.ObjectModel;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;

namespace EntraIDAuth.WebAssembly.Services;

/// <summary>
///     Classe d'appel à la Web API du projet.
/// </summary>
/// <param name="http"></param>
public class WebAPIClient(HttpClient http)
{
    public async Task<WeatherForecast[]> GetForecastAsync()
    {
        WeatherForecast[] results = [];

        try
        {
            return await http.GetFromJsonAsync<WeatherForecast[]>(
                "WeatherForecast") ?? [];
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }

        return results;
    }

    public async Task<WeatherForecast[]> GetForecastAdminAsync()
    {
        WeatherForecast[] results = [];

        try
        {
            return await http.GetFromJsonAsync<WeatherForecast[]>(
                "WeatherForecast/onlyadmin") ?? [];
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }

        return results;
    }
}
