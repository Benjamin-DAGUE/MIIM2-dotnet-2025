using Keepass.DBLib;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public static class Program
{
    public static void Main(string[] args)
    {
        InitDB();

        using KeepassDBContext dBContext = new();

        Vault vault = dBContext.Vaults.First();


        vault.Name = "Changed";

        EntityEntry<Vault> entry = dBContext.Entry(vault);
        dBContext.SaveChanges();

        Console.ReadKey();
    }

    public static void InitDB()
    {
        using KeepassDBContext dBContext = new();

#if DEBUG
        dBContext.Database.EnsureDeleted();
        dBContext.Database.EnsureCreated();
#endif

        AppUser appUser = new()
        {
            EntraID = Guid.NewGuid()
        };

        dBContext.Users.Add(appUser);

        Vault vault = new()
        {
            Creator = appUser,
            Name = "Test"
        };

        dBContext.Vaults.Add(vault);

        dBContext.SaveChanges();
    }

}



