using DB = Keepass.DBLib;
using DTO = Keepass.WebAPI.ObjectModel;

namespace Keepass.WebAPI.Converters;

//Implémente la logique de conversion entre le modèle DTO et le modèle DB
public static class VaultConverter
{
    #region Methods

    public static DTO.Vault ToDTO(this DB.Vault vault) => ToDTO(vault, null);

    public static DTO.Vault ToDTO(this DB.Vault vault, Action<DTO.Vault>? preparedCallback)
    {
        DTO.Vault dtoVault = new()
        {
            Id = vault.Id,
            Name = vault.Name,
            Description = vault.Description,
        };

        preparedCallback?.Invoke(dtoVault);

        return dtoVault;
    }

    public static DB.Vault ToDB(this DTO.AddVaultQuery addVaultQuery)
    {
        return new DB.Vault
        {
            Id = Guid.NewGuid(),
            Name = addVaultQuery.Name,
            Description = addVaultQuery.Description
        };
    }

    #endregion
}