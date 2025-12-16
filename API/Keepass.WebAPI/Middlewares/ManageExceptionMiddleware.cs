using Keepass.WebAPI.Exceptions;
using Keepass.WebAPI.ObjectModel;

namespace Keepass.WebAPI.Middlewares;

//Le middleware catch les exceptions pour :
//- Ajouter du log
//- Retourner un code erreur propre avec des messages d'erreurs spécifiques.
public class ManageExceptionMiddleware(ILogger<ManageExceptionMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            //next() permet d'appeler l'action suivante du pipeline ASP NET (dépend de l'ordre défini dans Program.cs)
            await next(context);
        }
        catch (ApiException ex)
        {
            ErrorMessage errorMessage = ex.ToObjectModel();

            logger.LogError(ex, "Error calling route {route}{newline}{error}", context.Request.Path, Environment.NewLine, errorMessage.ToString());
            context.Response.StatusCode = ex.StatusCode;
            await context.Response.WriteAsJsonAsync(errorMessage);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Internal server error");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(new ErrorMessage()
            {
                Code = ErrorCode.Undefined,
                Message = $"Internal server error"
            });
        }
    }
}
