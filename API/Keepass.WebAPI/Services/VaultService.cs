using DB = Keepass.DBLib;
using Keepass.WebAPI.Converters;
using Keepass.WebAPI.Exceptions;
using Keepass.WebAPI.Repositories;
using DTO = Keepass.WebAPI.ObjectModel;

namespace Keepass.WebAPI.Services;

public class VaultService(AppUserService appUserService, VaultRepository vaultRepository)
{
    #region Methods

    public async Task<DTO.Vault> GetForUserAsync(Guid vaultId)
    {
        DB.Vault vault = await vaultRepository.GetAsync(appUserService.CurrentAppUserId, vaultId)
            ?? throw new VaultNotFoundException(vaultId);

        return vault.ToDTO();
    }

    public async Task<IEnumerable<DTO.Vault>> GetAllForUserAsync() => (await vaultRepository.GetAllAsync(appUserService.CurrentAppUserId)).Select(c => c.ToDTO());

    public async Task<DTO.Vault> AddForUserAsync(DTO.AddVaultQuery vault)
    {
        DB.Vault toAddVault = vault.ToDB();
        toAddVault.AppUserId = appUserService.CurrentAppUserId;
        
        DB.Vault addedVault = await vaultRepository.AddAsync(toAddVault);

        return addedVault.ToDTO();
    }

    public async Task UpdateForUserAsync(DTO.Vault vault)
    {
        bool result = await vaultRepository.UpdateAsync(appUserService.CurrentAppUserId, vault.Id, vault.Name, vault.Description);

        if (result == false)
        {
            throw new VaultNotFoundException(vault.Id);
        }
    }

    public async Task DeleteForUserAsync(Guid vaultId)
    {
        bool result = await vaultRepository.DeleteAsync(appUserService.CurrentAppUserId, vaultId);

        if (result == false)
        {
            throw new VaultNotFoundException(vaultId);
        }
    }

    #endregion
}

