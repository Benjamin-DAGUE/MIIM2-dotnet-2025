using Keepass.WebAPI.ObjectModel;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Keepass.WebApp.Core;

public static partial class HttpRequestExceptionExtensions
{
    #region Fields

    private static readonly JsonSerializerOptions _option = new(JsonSerializerOptions.Default)
    {
        PropertyNameCaseInsensitive = true,
    };

    #endregion

    #region Methods

    public static ErrorMessage? TryGetErrorMessage(this HttpRequestException exception)
    {
        ErrorMessage? error = null;

        try
        {
            if (string.IsNullOrWhiteSpace(exception.Message) == false)
            {
                Match match = ExtractJson().Match(exception.Message);

                if (match.Success)
                {
                    string jsonError = match.Value;

                    error = JsonSerializer.Deserialize<ErrorMessage>(jsonError, _option);
                }
            }
        }
        catch (Exception ex)
        {

        }

        return error;
    }

    [GeneratedRegex("{.*}")]
    private static partial Regex ExtractJson();

    #endregion
}
