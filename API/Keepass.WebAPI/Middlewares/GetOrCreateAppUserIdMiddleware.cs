
using Keepass.WebAPI.Controllers;
using Keepass.WebAPI.Services;
using Microsoft.Identity.Web;

namespace Keepass.WebAPI.Middlewares;

public class GetOrCreateAppUserIdMiddleware(AppUserService appUserService, ILogger<VaultController> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await next(context);
        try
        {
            if ((context.User.Identity?.IsAuthenticated ?? false) == false
                ||
                Guid.TryParse(context.User.GetObjectId(), out Guid externalUserId) == false)
            {
                //Si l'utilisateur n'est pas auth ou que l'on arrive pas à récupérer l'Id Entra de l'utilisateur, on retourne une 401.
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else
            {
                //On utilise le service AppUserService pour récupérer l'Id interne de l'utilisateur à partir de son Id Entra (l'utilisateur sera créé si besoin).
                appUserService.CurrentAppUserId = await appUserService.GetOrCreateAppUserIdAsync(externalUserId);
                await next(context);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Internal server error");
        }
    }
}
