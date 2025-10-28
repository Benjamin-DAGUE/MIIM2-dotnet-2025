namespace Keepass.DBLib;

public class AppUser
{
    public int Identifier { get; set; }
    public Guid EntraID { get; set; }

    public HashSet<Vault> Vaults { get; set; } = [];
    public HashSet<Vault> SharedVaults { get; set; } = [];
}
