using Keepass.WebAPI.ObjectModel;

namespace Keepass.WebAPI.Exceptions;

public class VaultNotFoundException(Guid vaultId) : ApiException
{
    #region Properties

    public override int StatusCode => StatusCodes.Status404NotFound;

    public Guid VaultId { get; set; } = vaultId;

    #endregion

    #region Methods

    public override ErrorMessage ToObjectModel() => new()
    {
        Code = ErrorCode.VaultNotFound,
        Message = $"Vault with id {VaultId} has not been found."
    };

    #endregion
}   
