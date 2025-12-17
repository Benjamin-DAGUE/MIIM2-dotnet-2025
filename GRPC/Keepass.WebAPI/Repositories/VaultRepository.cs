using Keepass.DBLib;
using Microsoft.EntityFrameworkCore;

namespace Keepass.WebAPI.Repositories;

//Le repository se charge d'exécuter les requêtes SQL à l'aide de contextes temporaires.
public class VaultRepository(IDbContextFactory<KeepassDbContext> dbContextFactory)
{
    #region Methods

    public async Task<Vault?> GetAsync(Guid appUserId, Guid id)
    {
        using KeepassDbContext dbContext = await dbContextFactory.CreateDbContextAsync();

        return await dbContext
            .Vaults
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == id && v.AppUserId == appUserId);
    }

    public async Task<IEnumerable<Vault>> GetAllAsync(Guid appUserId)
    {
        using KeepassDbContext dbContext = await dbContextFactory.CreateDbContextAsync();

        return await dbContext
            .Vaults
            .AsNoTracking()
            .Where(v => v.AppUserId == appUserId)
            .ToListAsync();
    }

    public async Task<Vault> AddAsync(Vault vault)
    {
        using KeepassDbContext dbContext = await dbContextFactory.CreateDbContextAsync();
        await dbContext.Vaults.AddAsync(vault);
        await dbContext.SaveChangesAsync();

        return vault;
    }

    public async Task<bool> UpdateAsync(Guid appUserId, Guid id, string name, string? description)
    {
        using KeepassDbContext dbContext = await dbContextFactory.CreateDbContextAsync();

        int updatedCount = await dbContext
            .Vaults
            .Where(v => v.Id == id && v.AppUserId == appUserId)
            .ExecuteUpdateAsync(u => u
                .SetProperty(e => e.Name, name)
                .SetProperty(e => e.Description, description));

        return updatedCount > 0;
    }

    public async Task<bool> DeleteAsync(Guid appUserId, Guid id)
    {
        using KeepassDbContext dbContext = await dbContextFactory.CreateDbContextAsync();

        int deletedCount = await dbContext
            .Vaults
            .Where(v => v.Id == id && v.AppUserId == appUserId)
            .ExecuteDeleteAsync();

        return deletedCount > 0;
    }

    #endregion
}