using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Keepass.DBLib;
using Microsoft.EntityFrameworkCore;

namespace Keepass.RPCServer.Endpoints;
public class VaultEndpointService(IDbContextFactory<KeepassDbContext> dbContextFactory, ILogger<VaultEndpointService> logger) : VaultEndpoint.VaultEndpointBase
{
    public override async Task<GetVaultReply> Add(AddVaultRequest request, ServerCallContext context)
    {
        using KeepassDbContext dbContext = await dbContextFactory.CreateDbContextAsync();

        Guid appUserId = request.ApUserId.ToGuid() ?? throw new Exception("Unable to get AppUserId");

        AppUser? appUser = dbContext.AppUsers.FirstOrDefault(a => a.Id == appUserId);

        if(appUser == null)
        {
            appUser = new AppUser()
            {
                Id = appUserId,
                EntraId = Guid.NewGuid()
            };
        }

        Vault vault = new()
        {
            Creator = appUser,
            Name = request.Name,
            Description = request.Description,
        };

        dbContext.Vaults.Add(vault);
        dbContext.SaveChanges();

        return vault.ToVaultReply();
    }

    public override async Task<GetVaultReply> GetOne(GetOneVaultRequest request, ServerCallContext context)
    {
        using KeepassDbContext dbContext = await dbContextFactory.CreateDbContextAsync();

        Guid id = request.Id.ToGuid() ?? throw new Exception("Unable to get Id");

        return (await dbContext.Vaults.FindAsync(id))?.ToVaultReply() ?? throw new Exception("Not found");
    }

    public override async Task<GetAllVaultReply> GetAll(Empty request, ServerCallContext context)
    {
        using KeepassDbContext dbContext = await dbContextFactory.CreateDbContextAsync();

        GetAllVaultReply reply = new();

        IEnumerable<GetVaultReply> vaults = (await dbContext.Vaults.ToListAsync()).Select(v => v.ToVaultReply()) ?? throw new Exception("Not found");

        reply.Vaults.AddRange(vaults);

        return reply;
    }
}

public static class VaultExtensions
{
    public static GetVaultReply ToVaultReply(this Vault vault)
    {
        return new()
        {
            Id = ByteString.CopyFrom(vault.Id.ToByteArray()),
            ApUserId = ByteString.CopyFrom(vault.AppUserId.ToByteArray()),
            Name = vault.Name,
            Description = vault.Description,
        };
    }
}

public static class ByteStringExtensions
{
    public static Guid? ToGuid(this Google.Protobuf.ByteString bytes)
    {
        byte[] array = bytes.ToByteArray();

        if (array.Length == 16)
        {
            return new Guid(array);
        }

        return null;
    }
}