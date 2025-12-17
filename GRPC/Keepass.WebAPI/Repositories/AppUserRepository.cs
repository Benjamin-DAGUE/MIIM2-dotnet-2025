using Keepass.DBLib;
using Keepass.WebAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Keepass.WebAPI.Repositories;

//Le repository se charge d'exécuter les requêtes SQL à l'aide de contextes temporaires.
public class AppUserRepository(IDbContextFactory<KeepassDbContext> dbContextFactory, ILogger<AppUserRepository> logger)
{
    #region Methods

    public async Task<Guid> GetOrCreateAppUserIdAsync(Guid entraId)
    {
        using KeepassDbContext dbContext = await dbContextFactory.CreateDbContextAsync();

        using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            Guid appUserId = Guid.Empty;

            AppUser? appUser = await dbContext
                .AppUsers
                .FirstOrDefaultAsync(au => au.EntraId == entraId);

            if (appUser == null)
            {
                appUser = new AppUser()
                {
                    Id = Guid.NewGuid(),
                    EntraId = entraId
                };

                await dbContext.AddAsync(appUser);
                await dbContext.SaveChangesAsync();
            }

            await transaction.CommitAsync();

            appUserId = appUser.Id;

            return appUserId;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occured while geting or registering a user with external id {id}", entraId);
            await transaction.RollbackAsync();
            throw new UnableToGetOrRegisterAppUserException(entraId);
        }
    }

    #endregion
}
