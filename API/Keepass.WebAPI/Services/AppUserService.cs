using Keepass.WebAPI.Repositories;

namespace Keepass.WebAPI.Services;

public class AppUserService(AppUserRepository appUserRepository)
{
    #region Properties

    public Guid CurrentAppUserId { get; set; }

    #endregion

    #region Methods

    public async Task<Guid> GetOrCreateAppUserIdAsync(Guid externalUserId) => await appUserRepository.GetOrCreateAppUserIdAsync(externalUserId);
    
    #endregion
}