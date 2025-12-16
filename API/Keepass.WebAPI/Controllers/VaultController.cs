using Keepass.WebAPI.ObjectModel;
using Keepass.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace Keepass.WebAPI.Controllers;


//Le controller se charge d'exposer les routes.
//Les exceptions sont gérées dans le middleware ManageExceptionMiddleware
[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class VaultController(VaultService vaultService) : ControllerBase
{
    #region Methods

    [HttpGet()]
    public async Task<IActionResult> GetAll() => Ok(await vaultService.GetAllForUserAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(Guid id) => Ok(await vaultService.GetForUserAsync(id));

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddVaultQuery addVaultQuery) => Ok(await vaultService.AddForUserAsync(addVaultQuery));

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Vault vault)
    {
        await vaultService.UpdateForUserAsync(vault);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await vaultService.DeleteForUserAsync(id);
        return Ok();
    }

    #endregion
}
